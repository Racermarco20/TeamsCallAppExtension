using System.Diagnostics;

namespace TeamsCallApp
{
    public partial class CaptureForm : Form
    {
        public CaptureForm(string selectedText)
        {
            InitializeComponent();
            textBox1.Text = selectedText;
            this.AcceptButton = this.buttonCallNow;
        }

        private void buttonCallNow_Click(object sender, EventArgs e)
        {
            var phoneNumber = textBox1.Text;
            if (PhoneNumberValidator.IsPhoneNumber(phoneNumber))
            {

                MainForm.pNotifyIcon.ShowBalloonTip(2000, Program.APP_NAME, "Calling " + phoneNumber, ToolTipIcon.Info);

                Process.Start(new ProcessStartInfo($"tel:{phoneNumber}") { UseShellExecute = true });
                Close(); 
            }
            else
            {
                MessageBox.Show("Not a valid phone number!");
            }
        }
    }
}
