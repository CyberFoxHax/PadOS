using System.Xml;

namespace PadOS.SaveData.ProfileXML
{
    public class PadOSTrigger : ITrigger {
        public PadOSTriggerEnum Key { get; set; }

        public enum PadOSTriggerEnum {
            ProfileEnabled
        }
    }
}