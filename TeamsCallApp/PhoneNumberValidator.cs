using System.Text.RegularExpressions;

namespace TeamsCallApp
{
    public static class PhoneNumberValidator
    {
        public static bool IsPhoneNumber(string text)
        {
            return Regex.IsMatch(text, @"^\+?[1-9]\d{1,14}$");
        }
    }
}
