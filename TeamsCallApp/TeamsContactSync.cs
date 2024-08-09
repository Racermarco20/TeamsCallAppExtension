using Azure.Identity;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamsCallApp
{
    public class TeamsContactSync
    {
        private static string _clientId = "c6b2eb43-f857-474f-b9cf-50400edbce1f";  // From Azure AD App Registration
        private static string _tenantId = "9cc9a5f7-d74f-4822-bee4-efbd22852c48";  // From Azure AD App Registration
        private static string[] _scopes = new[] { "User.Read", "Contacts.Read" };

        public static async Task<List<Contact>> GetTeamsContactsAsync()
        {
            var options = new InteractiveBrowserCredentialOptions
            {
                ClientId = _clientId,
                TenantId = _tenantId,
                RedirectUri = new Uri("http://localhost")
            };

            var credential = new InteractiveBrowserCredential(options);

            var graphClient = new GraphServiceClient(credential, _scopes);

            // Fetch the contacts directly with GetAsync()
            var contacts = await graphClient.Me.Contacts.GetAsync();

            return contacts.Value.Select(c => new Contact
            {
                Name = c.DisplayName,
                PhoneNumber = c.MobilePhone
            }).ToList();
        }
    }
}