using System;
using System.Windows.Forms;

namespace TeamsCallApp
{
    public partial class SettingsForm : Form
    {
        public Keys CaptureHotkey { get; private set; }

        public SettingsForm(Keys currentHotkey)
        {
            InitializeComponent();
            CaptureHotkey = currentHotkey;
            textBoxHotkey.Text = CaptureHotkey.ToString();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (Enum.TryParse(textBoxHotkey.Text, true, out Keys newHotkey))
            {
                CaptureHotkey = newHotkey;
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
