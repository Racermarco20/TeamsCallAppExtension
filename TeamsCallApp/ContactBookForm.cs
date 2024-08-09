using System.Diagnostics;
using System.Text.Json;

namespace TeamsCallApp
{
    public partial class ContactBookForm : Form
    {
        private List<Contact> _contacts;
        private string _contactsFilePath;

        public ContactBookForm(string contactsFilePath)
        {
            InitializeComponent();
            _contactsFilePath = contactsFilePath;
            _contacts = LoadContacts();
            LoadContactsIntoListBox();
        }

        private async void LoadContactsIntoListBox()
        {
            listBoxContacts.Items.Clear();

            // Load existing contacts from local storage
            foreach (var contact in _contacts)
            {
                listBoxContacts.Items.Add($"{contact.Name} | {contact.PhoneNumber}");
            }
        }


        private void buttonAddContact_Click(object sender, EventArgs e)
        {
            var contact = new Contact
            {
                Name = textBoxName.Text,
                PhoneNumber = textBoxPhoneNumber.Text
            };

            _contacts.Add(contact);
            LoadContactsIntoListBox();

            textBoxName.Clear();
            textBoxPhoneNumber.Clear();
        }

        private void buttonDeleteContact_Click(object sender, EventArgs e)
        {
            var selectedIndex = listBoxContacts.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < _contacts.Count)
            {
                _contacts.RemoveAt(selectedIndex);
                LoadContactsIntoListBox();
            }
        }

        private void buttonCallContact_Click(object sender, EventArgs e)
        {
            var selectedIndex = listBoxContacts.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < _contacts.Count)
            {
                var selectedContact = _contacts[selectedIndex];
                InitiateCall(selectedContact.PhoneNumber);
            }
            else
            {
                MessageBox.Show("Please select a contact to call.", "No Contact Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void InitiateCall(string phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                try
                {
                    Process.Start(new ProcessStartInfo($"tel:{phoneNumber}") { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to initiate the call: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The phone number is invalid.", "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonSaveContacts_Click(object sender, EventArgs e)
        {
            SaveContacts();
        }

        private void SaveContacts()
        {
            var json = JsonSerializer.Serialize(_contacts);
            File.WriteAllText(_contactsFilePath, json);
            MessageBox.Show("Contacts saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private List<Contact> LoadContacts()
        {
            if (File.Exists(_contactsFilePath))
            {
                var json = File.ReadAllText(_contactsFilePath);
                return JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
            }
            return new List<Contact>();
        }

        private async void buttonSyncTeamsAccounts_Click(object sender, EventArgs e) { 
            LoadContactsIntoListBox();
            // Sync with Microsoft Teams contacts
            try
            {
                var teamsContacts = await TeamsContactSync.GetTeamsContactsAsync();
                foreach (var contact in teamsContacts)
                {
                    if (!_contacts.Any(c => c.Name == contact.Name && c.PhoneNumber == contact.PhoneNumber))
                    {
                        _contacts.Add(contact);
                        listBoxContacts.Items.Add($"{contact.Name} | {contact.PhoneNumber}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to sync with Teams contacts: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
