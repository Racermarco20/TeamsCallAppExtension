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
        private System.Windows.Forms.Label labelCallApp;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            label1 = new Label();
            textBoxHotkey = new TextBox();
            buttonSave = new Button();
            checkBoxStartWithWindows = new CheckBox();
            textBoxCallApp = new TextBox();
            labelCallApp = new Label();
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
            buttonSave.Location = new Point(140, 141);
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
            // labelCallApp
            // 
            labelCallApp.AutoSize = true;
            labelCallApp.Location = new Point(12, 78);
            labelCallApp.Name = "labelCallApp";
            labelCallApp.Size = new Size(81, 15);
            labelCallApp.TabIndex = 3;
            labelCallApp.Text = "Call App (URI)";
            // 
            // SettingsForm
            // 
            ClientSize = new Size(260, 171);
            Controls.Add(buttonSave);
            Controls.Add(textBoxCallApp);
            Controls.Add(labelCallApp);
            Controls.Add(checkBoxStartWithWindows);
            Controls.Add(textBoxHotkey);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "SettingsForm";
            Text = "Settings";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
