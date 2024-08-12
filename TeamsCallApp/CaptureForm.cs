using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

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
            string phoneNumber = textBox1.Text;
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                if (isValidPhoneNumber(phoneNumber))
                {
                    Process.Start(new ProcessStartInfo($"tel:{phoneNumber}") { UseShellExecute = true });
                    Close();
                }
                else
                {
                    MessageBox.Show("Not a valid phone number!");
                }
            }
            else
            {
                MessageBox.Show("You need to enter or copy a phone number!");
            }
        }

        private bool isValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^\+?(\d{1,3})?(\(\d{1,4}\))?\d{1,14}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }
    }
}
