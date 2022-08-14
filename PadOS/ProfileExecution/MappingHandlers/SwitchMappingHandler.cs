using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadOS.ProfileExecution {
    public class SwitchMappingHandler {
        private List<IActionHandler> _actions = new List<IActionHandler>();

        public void Add(IActionHandler action) {
            _actions.Add(action);
        }

        public void Invoke(int index) {
            _actions[index].Invoke();
            _actions[index].InvokeOff();
        }
    }
}
