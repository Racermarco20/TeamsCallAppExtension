using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace TeamsCallApp
{
    public static class ClipboardHelper
    {
        private const byte VK_CONTROL = 0x11;
        private const byte VK_C = 0x43;

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        public static async Task<string> GetSelectedTextAsync()
        {
            string selectedText = string.Empty;

            try
            {
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        selectedText = Clipboard.GetText();
                        if (!string.IsNullOrEmpty(selectedText))
                        {
                            break;
                        }
                    }
                    catch (ExternalException)
                    {
                        // Warte kurz und versuche es erneut
                        await Task.Delay(50);
                    }
                }
            }
            catch (ExternalException ex)
            {
                MessageBox.Show($"Fehler beim Zugriff auf die Zwischenablage: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return selectedText;
        }

    }
}
