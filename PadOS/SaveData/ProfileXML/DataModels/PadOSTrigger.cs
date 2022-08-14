using System.Xml;

namespace PadOS.SaveData.ProfileXML
{
    public class PadOSTrigger : ITrigger {
        public EPadOSTrigger Key { get; set; }
        public float Delay { get; set; }

        public enum EPadOSTrigger {
            ProfileEnabled,
            ProfileDisabled
        }
    }
}