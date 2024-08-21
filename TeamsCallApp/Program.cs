using System;
using System.Windows.Forms;

namespace TeamsCallApp
{
    internal static class Program
    {

        public static string APP_NAME = "Teams Call App";
        private static Mutex mutex = new Mutex(true, "{1E33349F-33C7-4061-88C5-70358ED4190F}");

        [STAThread]
        static void Main()
        {

            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("Die Anwendung läuft bereits.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
    }
}
