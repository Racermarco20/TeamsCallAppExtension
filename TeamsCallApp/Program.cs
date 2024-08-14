using System;
using System.Windows.Forms;

namespace TeamsCallApp
{
    internal static class Program
    {

        public static string APP_NAME = "Teamscall App";

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
