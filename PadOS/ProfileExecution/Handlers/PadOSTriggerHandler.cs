using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadOS.Input.GamePadInput;
using PadOS.SaveData.ProfileXML;

namespace PadOS.ProfileExecution {
    public class PadOSTriggerHandler : ITriggerHandler {
        private bool _enabled;
        public bool Enabled {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public event Action OnTrigger;

        public void Init(ITrigger node, GamePadInput input) {
            
        }
    }
}
