using System.Diagnostics;
using System.Windows.Controls;

namespace TeamsCallApp
{
    public static class ContextMenuHelper
    {
        private static ContextMenu _currentContextMenu;

        public static void ShowContextMenu(string phoneNumber)
        {
            if (_currentContextMenu != null && _currentContextMenu.IsOpen)
            {
                _currentContextMenu.IsOpen = false;
                _currentContextMenu = null;
            }

            _currentContextMenu = new ContextMenu();
            var callItem = new MenuItem { Header = "Call with Teams" };
            callItem.Click += (sender, e) => new CaptureForm(phoneNumber).Show();
            _currentContextMenu.Items.Add(callItem);

            _currentContextMenu.IsOpen = true;
        }
    }
}
