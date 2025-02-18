using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadOS.ProfileExecution {
    public class SwitchMappingHandler {
        private List<IActionHandler> _actions = new List<IActionHandler>();

        public bool AnyDown { get; private set; }

        public void Add(IActionHandler action) {
            _actions.Add(action);
        }

        public void Invoke(EventData p, int index) {
            AnyDown = true;
            _actions[index].Invoke(p);
            _actions[index].InvokeOff(p);
        }

        public void InvokeOff(EventData p) {
            AnyDown = false;
        }
    }
}
