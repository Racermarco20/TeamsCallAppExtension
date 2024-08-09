namespace TeamsCallApp
{
    partial class ContactBookForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox listBoxContacts;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.TextBox textBoxPhoneNumber;
        private System.Windows.Forms.Button buttonAddContact;
        private System.Windows.Forms.Button buttonDeleteContact;
        private System.Windows.Forms.Button buttonCallContact;
        private System.Windows.Forms.Button buttonSaveContacts;
        private System.Windows.Forms.Button buttonSyncTeamsAccounts;

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
            buttonSaveContacts = new Button();
            listBoxContacts = new ListBox();
            textBoxName = new TextBox();
            textBoxPhoneNumber = new TextBox();
            buttonAddContact = new Button();
            buttonDeleteContact = new Button();
            buttonCallContact = new Button();
            buttonSyncTeamsAccounts = new Button();
            SuspendLayout();
            // 
            // buttonSaveContacts
            // 
            buttonSaveContacts.BackColor = Color.WhiteSmoke;
            buttonSaveContacts.FlatAppearance.BorderSize = 1;
            buttonSaveContacts.FlatAppearance.MouseOverBackColor = Color.AntiqueWhite;
            buttonSaveContacts.FlatStyle = FlatStyle.Popup;
            buttonSaveContacts.Font = new Font("Segoe UI", 9F);
            buttonSaveContacts.ForeColor = Color.Black;
            buttonSaveContacts.Location = new Point(12, 340);
            buttonSaveContacts.Name = "buttonSaveContacts";
            buttonSaveContacts.Size = new Size(280, 30);
            buttonSaveContacts.TabIndex = 7;
            buttonSaveContacts.Text = "Save";
            buttonSaveContacts.UseVisualStyleBackColor = false;
            buttonSaveContacts.Click += buttonSaveContacts_Click;
            // 
            // listBoxContacts
            // 
            listBoxContacts.BackColor = Color.FromArgb(44, 62, 80);
            listBoxContacts.Font = new Font("Segoe UI", 9F);
            listBoxContacts.ForeColor = Color.White;
            listBoxContacts.FormattingEnabled = true;
            listBoxContacts.ItemHeight = 15;
            listBoxContacts.Location = new Point(12, 12);
            listBoxContacts.Name = "listBoxContacts";
            listBoxContacts.Size = new Size(280, 154);
            listBoxContacts.TabIndex = 0;
            // 
            // textBoxName
            // 
            textBoxName.BackColor = Color.FromArgb(44, 62, 80);
            textBoxName.BorderStyle = BorderStyle.FixedSingle;
            textBoxName.Font = new Font("Segoe UI", 9F);
            textBoxName.ForeColor = Color.White;
            textBoxName.Location = new Point(12, 180);
            textBoxName.Name = "textBoxName";
            textBoxName.PlaceholderText = "Name";
            textBoxName.Size = new Size(130, 23);
            textBoxName.TabIndex = 1;
            // 
            // textBoxPhoneNumber
            // 
            textBoxPhoneNumber.BackColor = Color.FromArgb(44, 62, 80);
            textBoxPhoneNumber.BorderStyle = BorderStyle.FixedSingle;
            textBoxPhoneNumber.Font = new Font("Segoe UI", 9F);
            textBoxPhoneNumber.ForeColor = Color.White;
            textBoxPhoneNumber.Location = new Point(162, 180);
            textBoxPhoneNumber.Name = "textBoxPhoneNumber";
            textBoxPhoneNumber.PlaceholderText = "Phone Number";
            textBoxPhoneNumber.Size = new Size(130, 23);
            textBoxPhoneNumber.TabIndex = 2;
            // 
            // buttonAddContact
            // 
            buttonAddContact.BackColor = Color.FromArgb(52, 152, 219);
            buttonAddContact.FlatAppearance.BorderSize = 0;
            buttonAddContact.FlatAppearance.MouseOverBackColor = Color.FromArgb(41, 128, 185);
            buttonAddContact.FlatStyle = FlatStyle.Flat;
            buttonAddContact.Font = new Font("Segoe UI", 9F);
            buttonAddContact.ForeColor = Color.White;
            buttonAddContact.Location = new Point(12, 220);
            buttonAddContact.Name = "buttonAddContact";
            buttonAddContact.Size = new Size(130, 30);
            buttonAddContact.TabIndex = 3;
            buttonAddContact.Text = "Add";
            buttonAddContact.UseVisualStyleBackColor = false;
            buttonAddContact.Click += buttonAddContact_Click;
            // 
            // buttonDeleteContact
            // 
            buttonDeleteContact.BackColor = Color.FromArgb(52, 152, 219);
            buttonDeleteContact.FlatAppearance.BorderSize = 0;
            buttonDeleteContact.FlatAppearance.MouseOverBackColor = Color.FromArgb(41, 128, 185);
            buttonDeleteContact.FlatStyle = FlatStyle.Flat;
            buttonDeleteContact.Font = new Font("Segoe UI", 9F);
            buttonDeleteContact.ForeColor = Color.White;
            buttonDeleteContact.Location = new Point(162, 220);
            buttonDeleteContact.Name = "buttonDeleteContact";
            buttonDeleteContact.Size = new Size(130, 30);
            buttonDeleteContact.TabIndex = 4;
            buttonDeleteContact.Text = "Delete";
            buttonDeleteContact.UseVisualStyleBackColor = false;
            buttonDeleteContact.Click += buttonDeleteContact_Click;
            // 
            // buttonCallContact
            // 
            buttonCallContact.BackColor = Color.FromArgb(52, 152, 219);
            buttonCallContact.FlatAppearance.BorderSize = 0;
            buttonCallContact.FlatAppearance.MouseOverBackColor = Color.FromArgb(41, 128, 185);
            buttonCallContact.FlatStyle = FlatStyle.Flat;
            buttonCallContact.Font = new Font("Segoe UI", 9F);
            buttonCallContact.ForeColor = Color.White;
            buttonCallContact.Location = new Point(12, 300);
            buttonCallContact.Name = "buttonCallContact";
            buttonCallContact.Size = new Size(280, 30);
            buttonCallContact.TabIndex = 6;
            buttonCallContact.Text = "Call";
            buttonCallContact.UseVisualStyleBackColor = false;
            buttonCallContact.Click += buttonCallContact_Click;
            // 
            // buttonSyncTeamsAccounts
            // 
            buttonSyncTeamsAccounts.BackColor = Color.FromArgb(52, 152, 219);
            buttonSyncTeamsAccounts.FlatAppearance.BorderSize = 0;
            buttonSyncTeamsAccounts.FlatAppearance.MouseOverBackColor = Color.FromArgb(41, 128, 185);
            buttonSyncTeamsAccounts.FlatStyle = FlatStyle.Flat;
            buttonSyncTeamsAccounts.Font = new Font("Segoe UI", 9F);
            buttonSyncTeamsAccounts.ForeColor = Color.White;
            buttonSyncTeamsAccounts.Location = new Point(12, 260);
            buttonSyncTeamsAccounts.Name = "buttonSyncTeamsAccounts";
            buttonSyncTeamsAccounts.Size = new Size(280, 30);
            buttonSyncTeamsAccounts.TabIndex = 5;
            buttonSyncTeamsAccounts.Text = "Sync Teams";
            buttonSyncTeamsAccounts.UseVisualStyleBackColor = false;
            buttonSyncTeamsAccounts.Click += buttonSyncTeamsAccounts_Click;
            // 
            // ContactBookForm
            // 
            BackColor = Color.FromArgb(52, 73, 94);
            ClientSize = new Size(310, 390);
            Controls.Add(buttonSaveContacts);
            Controls.Add(buttonCallContact);
            Controls.Add(buttonSyncTeamsAccounts);
            Controls.Add(buttonDeleteContact);
            Controls.Add(buttonAddContact);
            Controls.Add(textBoxPhoneNumber);
            Controls.Add(textBoxName);
            Controls.Add(listBoxContacts);
            Name = "ContactBookForm";
            Text = "Contact Book";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
