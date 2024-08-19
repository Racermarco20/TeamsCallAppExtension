using System;
using System.Windows.Forms;

namespace TeamsCallApp
{
    internal static class Program
    {

        public static string APP_NAME = "Teams Call App";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
