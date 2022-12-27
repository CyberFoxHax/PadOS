using System;

namespace PadOS.ProfileExecution {
    public interface ITriggerSwitchHandler : ITriggerInit{
        event Action<ITriggerSwitchHandler, int> OnTrigger;
        event Action<ITriggerSwitchHandler> OnTriggerOff;
        bool Enabled { get; set; }
    }
}
