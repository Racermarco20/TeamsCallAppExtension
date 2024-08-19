namespace TeamsCallApp
{
    partial class CaptureForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonCallNow;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaptureForm));
            textBox1 = new TextBox();
            buttonCallNow = new Button();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(260, 23);
            textBox1.TabIndex = 0;
            // 
            // buttonCallNow
            // 
            buttonCallNow.Location = new Point(197, 38);
            buttonCallNow.Name = "buttonCallNow";
            buttonCallNow.Size = new Size(75, 23);
            buttonCallNow.TabIndex = 1;
            buttonCallNow.Text = "Call Now";
            buttonCallNow.UseVisualStyleBackColor = true;
            buttonCallNow.Click += buttonCallNow_Click;
            // 
            // CaptureForm
            // 
            ClientSize = new Size(284, 71);
            Controls.Add(buttonCallNow);
            Controls.Add(textBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "CaptureForm";
            Text = "Capture Phone Number";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
