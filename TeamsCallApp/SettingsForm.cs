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
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (Enum.TryParse(textBoxHotkey.Text, true, out Keys newHotkey))
            {
                CaptureHotkey = newHotkey;
                StartWithWindows = checkBoxStartWithWindows.Checked;
                DefaultCallApp = textBoxCallApp.Text;

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Invalid hotkey. Please enter a valid key.");
            }
        }
    }
}
