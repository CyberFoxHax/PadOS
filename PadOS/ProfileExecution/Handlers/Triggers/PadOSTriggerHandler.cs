using System;
using System.Linq;
using System.Timers;
using PadOS.Input.GamePadInput;
using PadOS.SaveData.ProfileXML;

namespace PadOS.ProfileExecution {
    public class PadOSTriggerHandler : ITriggerHandler {
        private bool _enabled;
        public bool Enabled {
            get { return _enabled; }
            set { 
                _enabled = value;
                _timer.Start();
            }
        }

        public event TriggerEvent OnTrigger;
        public event TriggerEvent OnTriggerOff;

        private PadOSTrigger.EPadOSTrigger _key;
        private readonly Timer _timer = new Timer { AutoReset = false, Interval = 1 };

        public void Init(ITrigger p, GamePadInput input) {
            var node = (PadOSTrigger)p;
            _key = node.Name;
            if(node.Delay > 1)
                _timer.Interval = node.Delay;
            _timer.Elapsed += _timer_Elapsed;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e) {
            _timer.Stop();
            if (_enabled == false && _key == PadOSTrigger.EPadOSTrigger.ProfileDisabled) {
                OnTrigger?.Invoke(this);
                OnTriggerOff?.Invoke(this);
            }
            else if (_enabled && _key == PadOSTrigger.EPadOSTrigger.ProfileEnabled) {
                OnTrigger?.Invoke(this);
                OnTriggerOff?.Invoke(this);
            }
        }
    }
}
