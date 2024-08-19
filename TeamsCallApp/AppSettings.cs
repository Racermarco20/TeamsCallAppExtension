using System.Windows.Forms;

namespace TeamsCallApp
{
    public class AppSettings
    {
        public Keys CaptureHotkey { get; set; }
        public bool StartWithWindows { get; set; }
        public string CallAppUri { get; set; }
    }
}
