using System;

namespace PadOS.ProfileExecution {
    public interface ITriggerSwitchHandler {
        void Init(SaveData.ProfileXML.ITrigger node, Input.GamePadInput.GamePadInput input);
        event Action<int> OnTrigger;
        bool Enabled { get; set; }
    }
}
