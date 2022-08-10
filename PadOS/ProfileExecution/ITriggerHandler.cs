using System;

namespace PadOS.ProfileExecution {
    public interface ITriggerHandler {
        void Init(SaveData.ProfileXML.ITrigger node, Input.GamePadInput.GamePadInput input);
        event Action OnTrigger;
        event Action OnTriggerOff;
        bool Enabled { get; set; }
    }
}
