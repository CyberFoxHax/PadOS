using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadOS.Input.GamePadInput;
using PadOS.SaveData.ProfileXML;

namespace PadOS.ProfileExecution.Handlers {
    public class HoldSwitchHandler : ITriggerSwitchHandler {
        public bool Enabled { get; set; }

        public event Action<ITriggerSwitchHandler, int> OnTrigger;

        public void Init(ITrigger node, GamePadInput input) {
            
        }
    }
}
