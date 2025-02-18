using System;

namespace PadOS.ProfileExecution {
    public interface ITriggerSwitchHandler : ITriggerInit{
        event Action<EventData, ITriggerSwitchHandler, int> OnTrigger;
        event Action<EventData, ITriggerSwitchHandler> OnTriggerOff;
        bool Enabled { get; set; }
    }
}
