using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using Microsoft.Win32;

namespace TeamsCallApp
{
    public partial class MainForm : Form
    {
        private string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "settings.json");
        public static NotifyIcon pNotifyIcon = new NotifyIcon();

        private const int WM_HOTKEY = 0x0312;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int HOTKEY_ID = 1;
        private const string TelPrefix = "tel:";
        private const int WH_MOUSE_LL = 14;

        private Keys _currentHotkey = Keys.P;
        private bool _startWithWindows;
        private string _callAppUri = TelPrefix;
        private AppSettings _settings = new AppSettings();
        private bool _isLoaded = false;

        private static IntPtr _hookID = IntPtr.Zero;
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private static LowLevelMouseProc _proc = HookCallback;

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

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        public MainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Visible = true;

            pNotifyIcon = notifyIcon1;

            LoadSettings();
            SetStartup(_startWithWindows);
            RegisterHotKey(this.Handle, HOTKEY_ID, 0, (int)_currentHotkey);
        }

            private void SetStartup(bool enable)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (enable)
                {
                    key.SetValue(Program.APP_NAME, "\"" + Application.ExecutablePath + "\"");
                }
                else
                {
                    key.DeleteValue(Program.APP_NAME, false);
                }
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_RBUTTONDOWN)
            {
                HandleRightClick();
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private static void HandleRightClick()
        {
            var selectedText = ClipboardHelper.GetSelectedText();
            if (!string.IsNullOrEmpty(selectedText) && PhoneNumberValidator.IsPhoneNumber(selectedText))
            {
                ContextMenuHelper.ShowContextMenu(selectedText);
            }
        }

        private void LoadSettings()
        {
            if (File.Exists(configPath))
            {
                _settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(configPath));
                _currentHotkey = _settings.CaptureHotkey;
                _startWithWindows = _settings.StartWithWindows;
                _callAppUri = _settings.CallAppUri;
            }
        }

        private void SaveSettings()
        {
            _settings.CaptureHotkey = _currentHotkey;
            _settings.StartWithWindows = _startWithWindows;
            _settings.CallAppUri = _callAppUri;

            Directory.CreateDirectory(Path.GetDirectoryName(configPath));
            File.WriteAllText(configPath, JsonSerializer.Serialize(_settings));
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, HOTKEY_ID);
            notifyIcon1.Visible = false;
            UnhookWindowsHookEx(_hookID);
            base.OnFormClosing(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID)
            {
                var selectedText = ClipboardHelper.GetSelectedText();
                if (!string.IsNullOrEmpty(selectedText))
                {
                    var captureForm = new CaptureForm(selectedText);
                    captureForm.Show();
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        private void openSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new SettingsForm(_currentHotkey, _startWithWindows, _callAppUri))
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    UnregisterHotKey(this.Handle, HOTKEY_ID);
                    _currentHotkey = settingsForm.CaptureHotkey;
                    _startWithWindows = settingsForm.StartWithWindows;
                    _callAppUri = settingsForm.DefaultCallApp;
                    RegisterHotKey(this.Handle, HOTKEY_ID, 0, (int)_currentHotkey);
                    SaveSettings();
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Application.Exit();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {

            if (_isLoaded && this.WindowState == FormWindowState.Minimized)
                
            {
                notifyIcon1.ShowBalloonTip(1000, Program.APP_NAME, Program.APP_NAME + " has been minimized to tray!", ToolTipIcon.Info);
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _hookID = SetHook(_proc);
            _isLoaded = true;
            MainForm_Resize(sender, e);
        }
    }
}
