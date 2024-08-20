using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TeamsCallApp
{
    public static class ContextMenuHelper
    {

        private static Image phoneImage;
        private static Image copyImage;
        private static Image cancelImage;

        private static ContextMenuStrip _currentContextMenu;


        static ContextMenuHelper()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string resourcesPath = Path.Combine(baseDirectory, "resources");

            phoneImage = LoadImage(Path.Combine(resourcesPath, "phone.png"));
            copyImage = LoadImage(Path.Combine(resourcesPath, "copy.png"));
            cancelImage = LoadImage(Path.Combine(resourcesPath, "cancel.png"));
        }

        public static void ShowContextMenu(string phoneNumber)
        {
            if (_currentContextMenu != null && _currentContextMenu.Visible)
            {
                _currentContextMenu.Close();
                _currentContextMenu = null;
            }

            _currentContextMenu = new ContextMenuStrip();

            var callItem = new ToolStripMenuItem("Call with Teams")
            {
                Image = phoneImage,
                ImageScaling = ToolStripItemImageScaling.SizeToFit
            };
            callItem.Click += (sender, e) => new CaptureForm(phoneNumber).Show();

            var copyItem = new ToolStripMenuItem("Copy Number")
            {
                Image = copyImage,
                ImageScaling = ToolStripItemImageScaling.SizeToFit
            };
            copyItem.Click += (sender, e) => Clipboard.SetText(phoneNumber);

            var cancelItem = new ToolStripMenuItem("Cancel")
            {
                Image = cancelImage,
                ImageScaling = ToolStripItemImageScaling.SizeToFit
            };

            _currentContextMenu.Items.Add(callItem);
            _currentContextMenu.Items.Add(copyItem);
            _currentContextMenu.Items.Add(new ToolStripSeparator());
            _currentContextMenu.Items.Add(cancelItem);

            int posX = Cursor.Position.X - _currentContextMenu.Width;
            int posY = Cursor.Position.Y;

            Point menuPosition = new Point(posX, posY);
            _currentContextMenu.Show(menuPosition);
        }

        private static Image LoadImage(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    MessageBox.Show($"Bilddatei nicht gefunden: {path}");
                    return null;
                }

                byte[] imageData;

                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    imageData = new byte[fs.Length];
                    fs.Read(imageData, 0, (int)fs.Length);
                }

                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    return Image.FromStream(ms);
                }
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show($"Das Bild konnte aufgrund eines Speicherproblems nicht geladen werden: {path}");
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden des Bildes: {ex.Message}");
                return null;
            }
        }
    }
}
