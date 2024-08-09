using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows.Forms;
using TeamsCallApp;

namespace TeamsCallApp
{
    public partial class Form1 : Form
    {
        private string ConfigDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config");
        private string SettingsFilePath => Path.Combine(ConfigDirectory, "settings.json");
        private string ContactsFilePath => Path.Combine(ConfigDirectory, "contacts.json");
        private int HOTKEY_ID = 1;
        private Keys currentHotkey = Keys.P;
        private const int WM_HOTKEY = 0x0312;
        private CaptureForm currentCaptureForm;
        private bool startWithWindows = false;
        private string callAppUri = "tel:";
        private bool enableNotifications = true;
        private AppSettings settings = new AppSettings();

        public Form1()
        {
            InitializeComponent();
            this.SizeChanged += Form1_Resize;
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Visible = true;

            LoadSettings();

            if (enableNotifications)
            {
                RegisterHotKey(this.Handle, HOTKEY_ID, 0, (int)currentHotkey);
            }
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
            if (enableNotifications && m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID)
            {
                string selectedText = GetSelectedText();
                if (!string.IsNullOrEmpty(selectedText))
                {
                    ShowCaptureForm(selectedText);
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        private void ShowCaptureForm(string selectedText)
        {
            if (currentCaptureForm == null)
            {
                currentCaptureForm = new CaptureForm(selectedText);
                currentCaptureForm.FormClosed += (s, args) => currentCaptureForm = null;
                currentCaptureForm.Show();

                if (enableNotifications)
                {
                    notifyIcon1.ShowBalloonTip(1000, "TeamsCallApp", "Calling " + selectedText, ToolTipIcon.Info);
                }
            }
            else
            {
                MessageBox.Show("A capture form is already open.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                    if (enableNotifications)
                    {
                        RegisterHotKey(this.Handle, HOTKEY_ID, 0, (int)currentHotkey);
                    }
                }

                SaveSettings();
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
                settings = JsonSerializer.Deserialize<AppSettings>(json);

                currentHotkey = settings.CaptureHotkey;
                startWithWindows = settings.StartWithWindows;
                callAppUri = settings.CallAppUri;
                enableNotifications = settings.EnableNotifications;
            }
        }

        private void SaveSettings()
        {
            settings.CaptureHotkey = currentHotkey;
            settings.StartWithWindows = startWithWindows;
            settings.CallAppUri = callAppUri;
            settings.EnableNotifications = enableNotifications;

            if (!Directory.Exists(ConfigDirectory))
            {
                Directory.CreateDirectory(ConfigDirectory);
            }

            var json = JsonSerializer.Serialize(settings);
            File.WriteAllText(SettingsFilePath, json);
        }

        private void contactBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var contactBookForm = new ContactBookForm(ContactsFilePath))
            {
                contactBookForm.ShowDialog();
            }
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);


        public class AppSettings
        {
            public Keys CaptureHotkey { get; set; }
            public bool StartWithWindows { get; set; }
            public string CallAppUri { get; set; }
            public bool EnableNotifications { get; set; }
        }


    }
}