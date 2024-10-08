﻿using System.Text.RegularExpressions;

namespace TeamsCallApp
{
    public static class PhoneNumberValidator
    {
        public static bool IsPhoneNumber(string text)
        {
            return Regex.IsMatch(text, @"^\+?[1-9]\d{0,14}\s?\d+(\(\d+\))?\d+-?\d*$");
        }
    }
}
