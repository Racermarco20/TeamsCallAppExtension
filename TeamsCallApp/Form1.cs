using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TeamsCallApp
{
    public partial class Form1 : Form
    {
        private string ConfigDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config");
        private string SettingsFilePath => Path.Combine(ConfigDirectory, "settings.json");
        private string appName = "TeamsCallApp";
        private string executablePath = Application.ExecutablePath;

        // Default values
        private int HOTKEY_ID = 1;
        private Keys currentHotkey = Keys.P;
        private const int WM_HOTKEY = 0x0312;
        private CaptureForm currentCaptureForm;
        private bool startWithWindows = false;
        private string callAppUri = "tel:";
        private bool enableNotifications = true;
        private bool confirmBeforeCalling = false;
        private string theme = "Light"; // Default to Light theme
        private AppSettings settings = new AppSettings();

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const byte VK_CONTROL = 0x11;
        private const byte VK_C = 0x43;

        public Form1()
        {
            InitializeComponent();
            this.SizeChanged += Form1_Resize;
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Visible = true;

            LoadSettings();
            SetStartup(startWithWindows);

            if (enableNotifications)
            {
                RegisterHotKey(this.Handle, HOTKEY_ID, 0, (int)currentHotkey);
            }
        }

        private void SetStartup(bool enable)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (enable)
                {
                    key.SetValue(appName, "\"" + executablePath + "\"");
                }
                else
                {
                    key.DeleteValue(appName, false);
                }
            }
        }

        private void ApplyTheme(string theme, Control.ControlCollection controlsSettings)
        {
            // Überprüfe, ob das Theme korrekt angewendet wird
            MessageBox.Show("Theme is being applied: " + theme);

            if (theme == "Dark")
            {
                this.BackColor = Color.Black;
                this.ForeColor = Color.White;
                ApplyThemeToControls(controlsSettings, Color.FromArgb(45, 45, 48), Color.White);
            }
            else if (theme == "Light")
            {
                this.BackColor = Color.White;
                this.ForeColor = Color.Black;
                ApplyThemeToControls(controlsSettings, Color.White, Color.Black);
            }
            else
            {
                MessageBox.Show("No theme error!");
            }

            this.BackColor = Color.Red;

            // Sicherstellen, dass das Formular und seine Steuerelemente neu gezeichnet werden
            this.Refresh();
        }

        private void ApplyThemeToControls(Control.ControlCollection controls, System.Drawing.Color backColor, System.Drawing.Color foreColor)
        {
            MessageBox.Show($"Number of controls: {controls.Count}");

            foreach (Control control in controls)
            {
                if (control is TextBox || control is ComboBox || control is ListBox)
                {
                    control.BackColor = backColor == Color.FromArgb(45, 45, 48) ? Color.FromArgb(30, 30, 30) : Color.White;
                    control.ForeColor = foreColor;
                }
                else if (control is Button button)
                {
                    button.BackColor = Color.FromArgb(28, 28, 28);
                    button.ForeColor = Color.White;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
                }
                else if (control is Label || control is CheckBox || control is RadioButton)
                {
                    control.ForeColor = foreColor;
                }
                else
                {
                    control.BackColor = backColor;
                    control.ForeColor = foreColor;
                }

                // Ausgabe für Debugging-Zwecke
                MessageBox.Show($"Control: {control.Name}, BackColor: {control.BackColor}, ForeColor: {control.ForeColor}");

                // Rekursives Anwenden auf alle untergeordneten Steuerelemente
                if (control.HasChildren)
                {
                    ApplyThemeToControls(control.Controls, backColor, foreColor);
                }
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
                    if (confirmBeforeCalling)
                    {
                        var result = MessageBox.Show($"Do you want to call {selectedText}?", "Confirm Call", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            if (isValidPhoneNumber(selectedText))
                            {
                                ShowCaptureForm(selectedText);
                            } else MessageBox.Show("Not a valid phone number!");
                            
                        }
                    }
                    else
                    {
                        ShowCaptureForm(selectedText);
                    }
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
            string selectedText = string.Empty;
            bool textCopied = false;

            keybd_event(VK_CONTROL, 0, 0, 0); // STRG drücken
            keybd_event(VK_C, 0, 0, 0); // C drücken
            keybd_event(VK_C, 0, 2, 0); // C loslassen
            keybd_event(VK_CONTROL, 0, 2, 0); // STRG loslassen

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    Thread.Sleep(50);
                    selectedText = Clipboard.GetText();

                    if (!string.IsNullOrEmpty(selectedText))
                    {
                        textCopied = true;
                        break;
                    }
                }
                catch (ExternalException)
                {
                    Thread.Sleep(50);
                }
            }

            if (!textCopied)
            {
                MessageBox.Show("Could not copy the selected text. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return selectedText;
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
            using (SettingsForm settingsForm = new SettingsForm(currentHotkey, startWithWindows, callAppUri, enableNotifications, confirmBeforeCalling, theme))
            {
                Control.ControlCollection controlsSettings = settingsForm.Controls;
                if (settingsForm.ShowDialog() == DialogResult.OK)
                { 
                    UnregisterHotKey(this.Handle, HOTKEY_ID);
                    currentHotkey = settingsForm.CaptureHotkey;
                    startWithWindows = settingsForm.StartWithWindows;
                    callAppUri = settingsForm.DefaultCallApp;
                    enableNotifications = settingsForm.EnableNotifications;
                    confirmBeforeCalling = settingsForm.ConfirmBeforeCalling;
                    theme = settingsForm.Theme;

                    MessageBox.Show(controlsSettings.Count.ToString());


                    ApplyTheme(theme, controlsSettings);

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
                confirmBeforeCalling = settings.ConfirmBeforeCalling;
                theme = settings.Theme;
            }
        }

        private void SaveSettings()
        {
            settings.CaptureHotkey = currentHotkey;
            settings.StartWithWindows = startWithWindows;
            settings.CallAppUri = callAppUri;
            settings.EnableNotifications = enableNotifications;
            settings.ConfirmBeforeCalling = confirmBeforeCalling;
            settings.Theme = theme;

            if (!Directory.Exists(ConfigDirectory))
            {
                Directory.CreateDirectory(ConfigDirectory);
            }

            var json = JsonSerializer.Serialize(settings);
            File.WriteAllText(SettingsFilePath, json);
        }

        private bool isValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^\+?(\d{1,3})?(\(\d{1,4}\))?\d{1,14}$";
            return Regex.IsMatch(phoneNumber, pattern);
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
            public bool ConfirmBeforeCalling { get; set; }
            public string Theme { get; set; }
        }
    }
}
