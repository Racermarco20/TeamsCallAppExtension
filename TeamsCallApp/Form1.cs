using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows.Forms;

namespace TeamsCallApp
{
    public partial class Form1 : Form
    {
        private string ConfigDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config");
        private string SettingsFilePath => Path.Combine(ConfigDirectory, "settings.json");
        private int HOTKEY_ID = 1;
        private Keys currentHotkey = Keys.P;
        private const int WM_HOTKEY = 0x0312;
        private CaptureForm currentCaptureForm;
        private bool startWithWindows = false;
        private string callAppUri = "tel:";
        private bool enableNotifications = true;

        public Form1()
        {
            InitializeComponent();
            this.SizeChanged += Form1_Resize;
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Visible = true;

            // Load settings from the settings file
            LoadSettings();

            RegisterHotKey(this.Handle, HOTKEY_ID, 0, (int)currentHotkey);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID)
            {
                string selectedText = GetSelectedText();
                if (!string.IsNullOrEmpty(selectedText))
                {
                    ShowCaptureForm(selectedText);
                }

                if (!enableNotifications)
                {
                    PostMessage(GetForegroundWindow(), WM_KEYUP, (IntPtr)currentHotkey, IntPtr.Zero);
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        private void ShowCaptureForm(string selectedText)
        {
            if (enableNotifications)
            {
                if (currentCaptureForm == null)
                {
                    currentCaptureForm = new CaptureForm(selectedText);
                    currentCaptureForm.FormClosed += (s, args) => currentCaptureForm = null;
                    currentCaptureForm.Show();

                    notifyIcon1.ShowBalloonTip(1000, "TeamsCallApp", "Calling " + selectedText, ToolTipIcon.Info);
                }
                else
                {
                    MessageBox.Show("A capture form is already open.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private string GetSelectedText()
        {
            return Clipboard.GetText();  // TODO: Change to marked text (global)!
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            notifyIcon1.ContextMenuStrip.Visible = false;
            notifyIcon1.ContextMenuStrip.Close();
            this.Close();
        }

        private void openSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SettingsForm settingsForm = new SettingsForm(currentHotkey, startWithWindows, callAppUri, enableNotifications))
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    UnregisterHotKey(this.Handle, HOTKEY_ID);
                    currentHotkey = settingsForm.CaptureHotkey;
                    startWithWindows = settingsForm.StartWithWindows;
                    callAppUri = settingsForm.DefaultCallApp;
                    enableNotifications = settingsForm.EnableNotifications;

                    RegisterHotKey(this.Handle, HOTKEY_ID, 0, (int)currentHotkey);

                    SaveSettings();
                }
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                notifyIcon1.ContextMenuStrip.Show(Cursor.Position);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
            this.Hide();

            RegisterHotKey(this.Handle, HOTKEY_ID, 0, (int)currentHotkey);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, HOTKEY_ID);
            notifyIcon1.Visible = false;
            base.OnFormClosing(e);
        }

        private void LoadSettings()
        {
            string configPath = Path.Combine(ConfigDirectory, SettingsFilePath);
            if (File.Exists(configPath))
            {
                var json = File.ReadAllText(configPath);
                var settings = JsonSerializer.Deserialize<AppSettings>(json);

                currentHotkey = settings.CaptureHotkey;
                startWithWindows = settings.StartWithWindows;
                callAppUri = settings.CallAppUri;
                enableNotifications = settings.EnableNotifications;
            }
        }

        private void SaveSettings()
        {
            if (!Directory.Exists(ConfigDirectory))
            {
                Directory.CreateDirectory(ConfigDirectory);
            }

            var settings = new AppSettings
            {
                CaptureHotkey = currentHotkey,
                StartWithWindows = startWithWindows,
                CallAppUri = callAppUri,
                EnableNotifications = enableNotifications
            };

            var json = JsonSerializer.Serialize(settings);
            File.WriteAllText(Path.Combine(ConfigDirectory, SettingsFilePath), json);
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private const uint WM_KEYDOWN = 0x0100;
        private const uint WM_KEYUP = 0x0101;
    }

    public class AppSettings
    {
        public Keys CaptureHotkey { get; set; }
        public bool StartWithWindows { get; set; }
        public string CallAppUri { get; set; }
        public bool EnableNotifications { get; set; }
    }
}
