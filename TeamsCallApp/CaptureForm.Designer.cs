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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonCallNow = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(260, 20);
            this.textBox1.TabIndex = 0;
            // 
            // buttonCallNow
            // 
            this.buttonCallNow.Location = new System.Drawing.Point(197, 38);
            this.buttonCallNow.Name = "buttonCallNow";
            this.buttonCallNow.Size = new System.Drawing.Size(75, 23);
            this.buttonCallNow.TabIndex = 1;
            this.buttonCallNow.Text = "Call Now";
            this.buttonCallNow.UseVisualStyleBackColor = true;
            this.buttonCallNow.Click += new System.EventHandler(this.buttonCallNow_Click);
            // 
            // CaptureForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 71);
            this.Controls.Add(this.buttonCallNow);
            this.Controls.Add(this.textBox1);
            this.Name = "CaptureForm";
            this.Text = "Capture Phone Number";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
