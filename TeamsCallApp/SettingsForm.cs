using System;
using System.Windows.Forms;

namespace TeamsCallApp
{
    public partial class SettingsForm : Form
    {
        public Keys CaptureHotkey { get; private set; }
        public bool StartWithWindows { get; private set; }
        public string DefaultDialingPrefix { get; private set; }
        public string DefaultCallApp { get; private set; }
        public bool EnableNotifications { get; private set; }
        public bool ConfirmBeforeCalling { get; private set; }
        public string Theme { get; private set; }

        public SettingsForm(Keys currentHotkey, bool startWithWindows, string callApp, bool notifications, bool confirmBeforeCalling, string theme)
        {
            InitializeComponent();
            CaptureHotkey = currentHotkey;
            textBoxHotkey.Text = CaptureHotkey.ToString();

            checkBoxStartWithWindows.Checked = startWithWindows;
            textBoxCallApp.Text = callApp;
            checkBoxEnableNotifications.Checked = notifications;
            checkBoxConfirmBeforeCalling.Checked = confirmBeforeCalling;


            this.Controls.Add(textBoxHotkey);
            this.Controls.Add(checkBoxStartWithWindows);
            this.Controls.Add(textBoxCallApp);
            this.Controls.Add(checkBoxEnableNotifications);
            this.Controls.Add(checkBoxConfirmBeforeCalling);
            this.Controls.Add(comboBoxTheme);

            comboBoxTheme.SelectedItem = theme;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (Enum.TryParse(textBoxHotkey.Text, true, out Keys newHotkey))
            {
                CaptureHotkey = newHotkey;
                StartWithWindows = checkBoxStartWithWindows.Checked;
                DefaultCallApp = textBoxCallApp.Text;
                EnableNotifications = checkBoxEnableNotifications.Checked;
                ConfirmBeforeCalling = checkBoxConfirmBeforeCalling.Checked;
                Theme = comboBoxTheme.SelectedItem.ToString();

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
