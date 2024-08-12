using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;

namespace TeamsCallApp
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxHotkey;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.CheckBox checkBoxStartWithWindows;
        private System.Windows.Forms.TextBox textBoxCallApp;
        private System.Windows.Forms.CheckBox checkBoxEnableNotifications;
        private System.Windows.Forms.Label labelCallApp;
        private System.Windows.Forms.Label labelNotifications;
        private System.Windows.Forms.CheckBox checkBoxConfirmBeforeCalling;
        private System.Windows.Forms.ComboBox comboBoxTheme;
        private System.Windows.Forms.Label labelTheme;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            label1 = new Label();
            textBoxHotkey = new TextBox();
            buttonSave = new Button();
            checkBoxStartWithWindows = new CheckBox();
            textBoxCallApp = new TextBox();
            checkBoxEnableNotifications = new CheckBox();
            labelCallApp = new Label();
            labelNotifications = new Label();
            checkBoxConfirmBeforeCalling = new CheckBox();
            comboBoxTheme = new ComboBox();
            labelTheme = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(90, 15);
            label1.TabIndex = 0;
            label1.Text = "Capture Hotkey";
            // 
            // textBoxHotkey
            // 
            textBoxHotkey.Location = new Point(140, 12);
            textBoxHotkey.Name = "textBoxHotkey";
            textBoxHotkey.Size = new Size(100, 23);
            textBoxHotkey.TabIndex = 1;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(140, 250);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(100, 23);
            buttonSave.TabIndex = 11;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // checkBoxStartWithWindows
            // 
            checkBoxStartWithWindows.AutoSize = true;
            checkBoxStartWithWindows.Location = new Point(12, 50);
            checkBoxStartWithWindows.Name = "checkBoxStartWithWindows";
            checkBoxStartWithWindows.Size = new Size(128, 19);
            checkBoxStartWithWindows.TabIndex = 2;
            checkBoxStartWithWindows.Text = "Start with Windows";
            checkBoxStartWithWindows.UseVisualStyleBackColor = true;
            // 
            // textBoxCallApp
            // 
            textBoxCallApp.Location = new Point(140, 75);
            textBoxCallApp.Name = "textBoxCallApp";
            textBoxCallApp.Size = new Size(100, 23);
            textBoxCallApp.TabIndex = 4;
            // 
            // checkBoxEnableNotifications
            // 
            checkBoxEnableNotifications.AutoSize = true;
            checkBoxEnableNotifications.Location = new Point(140, 110);
            checkBoxEnableNotifications.Name = "checkBoxEnableNotifications";
            checkBoxEnableNotifications.Size = new Size(61, 19);
            checkBoxEnableNotifications.TabIndex = 6;
            checkBoxEnableNotifications.Text = "Enable";
            checkBoxEnableNotifications.UseVisualStyleBackColor = true;
            // 
            // labelCallApp
            // 
            labelCallApp.AutoSize = true;
            labelCallApp.Location = new Point(12, 78);
            labelCallApp.Name = "labelCallApp";
            labelCallApp.Size = new Size(81, 15);
            labelCallApp.TabIndex = 3;
            labelCallApp.Text = "Call App (URI)";
            // 
            // labelNotifications
            // 
            labelNotifications.AutoSize = true;
            labelNotifications.Location = new Point(12, 111);
            labelNotifications.Name = "labelNotifications";
            labelNotifications.Size = new Size(75, 15);
            labelNotifications.TabIndex = 5;
            labelNotifications.Text = "Notifications";
            // 
            // checkBoxConfirmBeforeCalling
            // 
            checkBoxConfirmBeforeCalling.AutoSize = true;
            checkBoxConfirmBeforeCalling.Location = new Point(12, 145);
            checkBoxConfirmBeforeCalling.Name = "checkBoxConfirmBeforeCalling";
            checkBoxConfirmBeforeCalling.Size = new Size(143, 19);
            checkBoxConfirmBeforeCalling.TabIndex = 7;
            checkBoxConfirmBeforeCalling.Text = "Confirm Before Calling";
            checkBoxConfirmBeforeCalling.UseVisualStyleBackColor = true;
            // 
            // comboBoxTheme
            // 
            comboBoxTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTheme.FormattingEnabled = true;
            comboBoxTheme.Items.AddRange(new object[] { "Light", "Dark" });
            comboBoxTheme.Location = new Point(140, 180);
            comboBoxTheme.Name = "comboBoxTheme";
            comboBoxTheme.Size = new Size(100, 23);
            comboBoxTheme.TabIndex = 9;
            // 
            // labelTheme
            // 
            labelTheme.AutoSize = true;
            labelTheme.Location = new Point(12, 183);
            labelTheme.Name = "labelTheme";
            labelTheme.Size = new Size(43, 15);
            labelTheme.TabIndex = 8;
            labelTheme.Text = "Theme";
            // 
            // SettingsForm
            // 
            ClientSize = new Size(260, 290);
            Controls.Add(buttonSave);
            Controls.Add(checkBoxConfirmBeforeCalling);
            Controls.Add(comboBoxTheme);
            Controls.Add(labelTheme);
            Controls.Add(checkBoxEnableNotifications);
            Controls.Add(labelNotifications);
            Controls.Add(textBoxCallApp);
            Controls.Add(labelCallApp);
            Controls.Add(checkBoxStartWithWindows);
            Controls.Add(textBoxHotkey);
            Controls.Add(label1);
            Name = "SettingsForm";
            Text = "Settings";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
