
namespace PadOS.SaveData.ProfileXML {
    public class AnalogueTrigger : ITrigger {
        public float Value { get; set; } = 0.5f;
        public int Frequency { get; set; } = 0;
        public Input.GamePadInput.GamePadInput.Axis Axis { get; set; }
    }
}
