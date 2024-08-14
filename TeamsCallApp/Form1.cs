using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Windows; // für WPF-Controls
using System.Windows.Controls; // für WPF-Controls wie Button, Label
using System.Windows.Input; // für WPF-Ereignisse
using Clipboard = System.Windows.Clipboard; // Richtige Verwendung der Zwischenablage in WPF
using WinForms = System.Windows.Forms; // Alias für Windows Forms
using System.Windows.Forms; // für Windows Forms (ohne Alias)

namespace TeamsCallApp
{
    public partial class Form1 : WinForms.Form // Verwende den Alias für das Form
    {

        private string ConfigDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config");
        private string SettingsFilePath => Path.Combine(ConfigDirectory, "settings.json");
        private string appName = Program.APP_NAME;
        private string executablePath = WinForms.Application.ExecutablePath; // Verwende den Alias
        private static System.Windows.Controls.ContextMenu _currentContextMenu = null;

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

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const byte VK_CONTROL = 0x11;
        private const byte VK_C = 0x43;
        private const int WH_MOUSE_LL = 14;
        private const int WM_RBUTTONDOWN = 0x0204;
        private static LowLevelMouseProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        public Form1()
        {
            InitializeComponent();
            this.SizeChanged += Form1_Resize;
            this.WindowState = WinForms.FormWindowState.Minimized;
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

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                // Wenn das Kontextmenü geöffnet ist und ein Klick außerhalb erfolgt, schließen
                if (_currentContextMenu != null && _currentContextMenu.IsOpen)
                {
                    _currentContextMenu.IsOpen = false;
                    _currentContextMenu = null;
                }

                if (wParam == (IntPtr)WM_RBUTTONDOWN)
                {
                    HandleRightClick();
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private static void HandleRightClick()
        {
            string selectedText = GetSelectedText();
            if (!string.IsNullOrEmpty(selectedText) && IsPhoneNumber(selectedText))
            {
                // Schließe das aktuelle Kontextmenü, wenn es geöffnet ist
                if (_currentContextMenu != null && _currentContextMenu.IsOpen)
                {
                    _currentContextMenu.IsOpen = false;
                }

                ShowContextMenu(selectedText);
            }
        }

        private static string GetSelectedText()
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
            Clipboard.Clear();

            return selectedText;
        }

        private static bool IsPhoneNumber(string text)
        {
            // Regex, um zu überprüfen, ob der Text eine Telefonnummer ist
            return Regex.IsMatch(text, @"^\+?[1-9]\d{1,14}$");
        }

        private static void ShowContextMenu(string phoneNumber)
        {
            // WPF-Kontextmenü erstellen
            System.Windows.Controls.ContextMenu contextMenu = new System.Windows.Controls.ContextMenu(); // Verwende den WPF-Namensraum
            System.Windows.Controls.MenuItem callItem = new System.Windows.Controls.MenuItem { Header = "Call with Teams" }; // Verwende den WPF-Namensraum
            callItem.Click += (sender, e) => CallWithTeams(phoneNumber);
            contextMenu.Items.Add(callItem);

            // Kontextmenü anzeigen
            contextMenu.IsOpen = true;
        }

        private static void CallWithTeams(string phoneNumber)
        {
            Process.Start(new ProcessStartInfo("tel:" + phoneNumber) { UseShellExecute = true });
        }

        protected override void OnClosed(EventArgs e)
        {
            UnhookWindowsHookEx(_hookID);
            base.OnClosed(e);
        }

        protected override void WndProc(ref WinForms.Message m) // Verwende den Alias für Message
        {
            if (enableNotifications && m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID)
            {
                string selectedText = GetSelectedText();
                if (!string.IsNullOrEmpty(selectedText))
                {
                    if (confirmBeforeCalling)
                    {
                        var result = WinForms.MessageBox.Show($"Do you want to call {selectedText}?", "Confirm Call", WinForms.MessageBoxButtons.YesNo, WinForms.MessageBoxIcon.Question);
                        if (result == WinForms.DialogResult.Yes)
                        {
                            if (IsPhoneNumber(selectedText))
                            {
                                ShowCaptureForm(selectedText);
                            }
                            else WinForms.MessageBox.Show("Not a valid phone number!");

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
                    notifyIcon1.ShowBalloonTip(1000, "TeamsCallApp", "Calling " + selectedText, WinForms.ToolTipIcon.Info);
                }
            }
            else
            {
                WinForms.MessageBox.Show("A capture form is already open.", "Information", WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Information);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == WinForms.FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
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
                WinForms.Control.ControlCollection controlsSettings = settingsForm.Controls;
                if (settingsForm.ShowDialog() == WinForms.DialogResult.OK)
                {
                    UnregisterHotKey(this.Handle, HOTKEY_ID);
                    currentHotkey = settingsForm.CaptureHotkey;
                    startWithWindows = settingsForm.StartWithWindows;
                    callAppUri = settingsForm.DefaultCallApp;
                    enableNotifications = settingsForm.EnableNotifications;
                    confirmBeforeCalling = settingsForm.ConfirmBeforeCalling;
                    theme = settingsForm.Theme;

                    ApplyTheme(theme, controlsSettings);

                    if (enableNotifications)
                    {
                        RegisterHotKey(this.Handle, HOTKEY_ID, 0, (int)currentHotkey);
                    }
                }

                SaveSettings();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, WinForms.MouseEventArgs e) // Verwende den Alias
        {
            if (e.Button == WinForms.MouseButtons.Left)
            {
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseClick(object sender, WinForms.MouseEventArgs e) // Verwende den Alias
        {
            if (e.Button == WinForms.MouseButtons.Right)
            {
                notifyIcon1.ContextMenuStrip.Show(WinForms.Cursor.Position);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _hookID = SetHook(_proc);
            notifyIcon1.Visible = true;
            this.Hide();
        }

        protected override void OnFormClosing(WinForms.FormClosingEventArgs e) // Verwende den Alias
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

        private void ApplyTheme(string theme, WinForms.Control.ControlCollection controlsSettings) // Verwende den Alias
        {
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
                WinForms.MessageBox.Show("No theme error!");
            }

            this.Refresh();
        }

        private void ApplyThemeToControls(WinForms.Control.ControlCollection controls, System.Drawing.Color backColor, System.Drawing.Color foreColor) // Verwende den Alias
        {
            foreach (WinForms.Control control in controls) // Verwende den Alias
            {
                if (control is WinForms.TextBox || control is WinForms.ComboBox || control is WinForms.ListBox) // Verwende den Alias
                {
                    control.BackColor = backColor == Color.FromArgb(45, 45, 48) ? Color.FromArgb(30, 30, 30) : Color.White;
                    control.ForeColor = foreColor;
                }
                else if (control is WinForms.Button button) // Verwende den Alias
                {
                    button.BackColor = Color.FromArgb(28, 28, 28);
                    button.ForeColor = Color.White;
                    button.FlatStyle = WinForms.FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
                }
                else if (control is WinForms.Label || control is WinForms.CheckBox || control is WinForms.RadioButton) // Verwende den Alias
                {
                    control.ForeColor = foreColor;
                }
                else
                {
                    control.BackColor = backColor;
                    control.ForeColor = foreColor;
                }

                if (control.HasChildren)
                {
                    ApplyThemeToControls(control.Controls, backColor, foreColor); // Verwende den Alias
                }
            }
        }

        public class AppSettings
        {
            public Keys CaptureHotkey { get; set; }
            public bool StartWithWindows { get; set; }
            public string CallAppUri { get; set; }
            public bool EnableNotifications { get; set; }
            public bool ConfirmBeforeCalling { get; set; }
            public string Theme { get; set; }
        }

        private void InitializeNotifyIcon()
        {
            string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources", "microsoft_teams_icon_137398.ico");

            notifyIcon1 = new NotifyIcon
            {
                Icon = new System.Drawing.Icon(iconPath),
                ContextMenuStrip = contextMenuStrip1,
                Text = "TeamsCallApp",
                Visible = true
            };

            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
        }
    }
}
