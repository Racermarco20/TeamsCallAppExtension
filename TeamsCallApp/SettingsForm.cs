using System;
using System.Windows.Forms;

namespace TeamsCallApp
{
    public partial class SettingsForm : Form
    {
        public Keys CaptureHotkey { get; private set; }
        public bool StartWithWindows { get; private set; }
        public string DefaultCallApp { get; private set; }

        public SettingsForm(Keys currentHotkey, bool startWithWindows, string callApp)
        {
            InitializeComponent();
            CaptureHotkey = currentHotkey;
            textBoxHotkey.Text = CaptureHotkey.ToString();

            checkBoxStartWithWindows.Checked = startWithWindows;
            textBoxCallApp.Text = callApp;

            textBoxHotkey.KeyDown += TextBoxHotkey_KeyDown;
        }

        private void TextBoxHotkey_KeyDown(object sender, KeyEventArgs e)
        {
            textBoxHotkey.Text = e.KeyCode.ToString();
            CaptureHotkey = e.KeyCode;

            e.SuppressKeyPress = true;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            StartWithWindows = checkBoxStartWithWindows.Checked;
            DefaultCallApp = textBoxCallApp.Text;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
