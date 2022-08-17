using System;

namespace PadOS.ProfileExecution {
    public interface ITriggerHandler : ITriggerInit {
        event TriggerEvent OnTrigger;
        event TriggerEvent OnTriggerOff;
        bool Enabled { get; set; }
    }
}
