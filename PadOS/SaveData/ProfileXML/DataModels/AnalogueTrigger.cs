
namespace PadOS.SaveData.ProfileXML {
    public class AnalogueTrigger : ITrigger {
        public float Value { get; set; }
        public int Frequency { get; set; }
        public EAxis Axis { get; set; }

        public enum EAxis {
            RightThumbX, RightThumbY,
            LeftThumbX, LeftThumbY,
            RightTrigger, LeftTrigger
        }
    }
}
