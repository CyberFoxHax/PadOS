using System;

namespace PadOS.ProfileExecution {
    public interface ITriggerHandler {
        void Init(SaveData.ProfileXML.ITrigger node, Input.GamePadInput.GamePadInput input);
        event TriggerEvent OnTrigger;
        event TriggerEvent OnTriggerOff;
        bool Enabled { get; set; }
    }
}
