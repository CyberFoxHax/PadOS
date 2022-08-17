using System;

namespace PadOS.ProfileExecution {
    public interface ITriggerSwitchHandler : ITriggerInit{
        event Action<ITriggerSwitchHandler, int> OnTrigger;
        bool Enabled { get; set; }
    }
}
