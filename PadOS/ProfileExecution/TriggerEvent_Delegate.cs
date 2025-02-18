using static XInputDotNetPure.GamePadState;

namespace PadOS.ProfileExecution {
    public delegate void TriggerEvent(EventData sender);

    public class EventData {
        public ITriggerHandler Sender { get; set; }
        public int? PlayerIndex { get; set; }

        public ButtonsConstants? Buttons { get; set; }
        public bool? IsDownEvent { get; set; }

        public Input.GamePadInput.GamePadInput.Axis? Axis { get; set; }
        public float? AnalogueValue { get; set; }
    }

}
