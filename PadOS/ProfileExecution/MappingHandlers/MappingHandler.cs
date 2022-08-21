using System.Collections.Generic;

namespace PadOS.ProfileExecution {
    public class MappingHandler {
        private List<IActionHandler> _actions = new List<IActionHandler>();

        public void Add(IActionHandler action) {
            _actions.Add(action);
        }

        public void Invoke() {
            foreach (var item in _actions) {
                item.Invoke();
            }
        }
        public void InvokeOff() {
            foreach (var item in _actions) {
                item.InvokeOff();
            }
        }
    }
}
