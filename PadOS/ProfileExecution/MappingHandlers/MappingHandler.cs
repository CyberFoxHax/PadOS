using System.Collections.Generic;

namespace PadOS.ProfileExecution {
    public class MappingHandler {
        private List<IActionHandler> _actions = new List<IActionHandler>();

        public bool AnyDown { get; private set; }

        public void Add(IActionHandler action) {
            _actions.Add(action);
        }

        public void Invoke(EventData p) {
            AnyDown = true;
            foreach (var item in _actions) {
                item.Invoke(p);
            }
        }
        public void InvokeOff(EventData p) {
            AnyDown = false;
            foreach (var item in _actions) {
                item.InvokeOff(p);
            }
        }
    }
}
