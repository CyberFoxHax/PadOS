namespace PadOS.ProfileExecution {
    public delegate void TriggerEvent(ITriggerHandler sender);

    public class EventData {
        public ITriggerHandler Sender { get; set; }
        public int PlayerIndex { get; set; }
        public Input.GamePadInput.GamePadInput.ButtonKey Button { get; set; }
        public bool IsDownEvent { get; set; }
        public float AnalogueValue { get; set; }
    }

}
