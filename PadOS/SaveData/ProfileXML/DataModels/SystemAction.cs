using System.Xml;

namespace PadOS.SaveData.ProfileXML
{
    public class SystemAction : IAction {
        public string Key { get; set; } // Reboot, Shutdown, Lock
    }
}