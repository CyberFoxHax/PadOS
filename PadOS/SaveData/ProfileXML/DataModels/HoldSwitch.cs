using System.Xml;

namespace PadOS.SaveData.ProfileXML
{
    public class HoldSwitch : ITrigger {
        public int Delay { get; set; } = 1000;
        public string Button { get; set; }
    }
}