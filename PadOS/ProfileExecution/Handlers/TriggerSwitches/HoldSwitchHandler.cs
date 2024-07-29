using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using PadOS.Input.GamePadInput;
using PadOS.SaveData.ProfileXML;

namespace PadOS.ProfileExecution {
    // add default value if no timeout is defined
    public class HoldSwitchHandler : ITriggerSwitchHandler {
        private bool _enabled;
        public bool Enabled {
            get => _enabled;
            set {
                _enabled = value;
                foreach (var item in _triggerHandlers)
                    item.Enabled = value;
            }
        }

        public event Action<ITriggerSwitchHandler, int> OnTrigger;
        public event Action<ITriggerSwitchHandler> OnTriggerOff;

        private ITriggerHandler[] _triggerHandlers;
        private float[] _timeouts;
        private DateTime _startTime;
        private DateTime _endTime;
        private Timer _timer = new Timer { AutoReset = false };
        private bool _on;

        private int _buttonsDownCount = 0;
        private GamePadInput _gamePad;

        private async void VibrateHolds() {
            // doesn't quite work
            foreach (var t in _timeouts) {
                if (t == 0)
                    continue;
                await Task.Delay((int)t);
                if (_on == false)
                    break;
                _gamePad.SetVibration(0, 1, 1, 100);
            }
        }

        public void Init(ITrigger node, GamePadInput input) {
            _gamePad = input;
            var sw = node as HoldSwitch;
            _triggerHandlers = new ITriggerHandler[sw.Triggers.Count];
            _timeouts = new float[sw.Actions.Count];
            for (int i = 0; i < sw.Triggers.Count; i++) {
                _triggerHandlers[i] = Maps.TriggerHandlers.InitHandler(sw.Triggers[i].Owner, input);
                _triggerHandlers[i].OnTrigger += HoldSwitchHandler_OnTrigger;
                _triggerHandlers[i].OnTriggerOff += HoldSwitchHandler_OnTriggerOff;
            }
            for (int i = 0; i < sw.Actions.Count; i++) {
                _timeouts[i] = sw.Actions[i].Timeout;
            }
            _timer.Interval = _timeouts.Last();
            _timer.Elapsed += timer_Elapsed;
            _timer.Stop();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e) {
            Console.WriteLine("[PadOS] timer_Elapsed");
            _timer.Stop();
            _on = false;
            _endTime = DateTime.Now;
            var diff = (_endTime - _startTime).TotalMilliseconds;

            _endTime = default;
            _startTime = default;
            var index = _timeouts.Length - 1;
            OnTrigger?.Invoke(this, index);
        }

        private void HoldSwitchHandler_OnTriggerOff(ITriggerHandler sender) {
            Console.WriteLine("[PadOS] HoldSwitchHandler_OnTriggerOff");

            _buttonsDownCount--;
            if (_buttonsDownCount == 0) {
                OnTriggerOff?.Invoke(this);
            }
            if (_on == false)
                return;
            _timer.Stop();
            _on = false;
            _endTime = DateTime.Now;

            var diff = (_endTime - _startTime).TotalMilliseconds;
            var index = 0;
            float t = 0;
            if (diff > _timeouts[1]) {
                for (int i = _timeouts.Length - 1; i >= 0; i--)
                    if (diff > _timeouts[i]) {
                        index = i;
                        break;
                    }
            }


            _endTime = default;
            _startTime = default;
            OnTrigger?.Invoke(this, index);
        }

        private void HoldSwitchHandler_OnTrigger(ITriggerHandler sender) {
            _buttonsDownCount++;
            _startTime = DateTime.Now;
            _on = true;
            VibrateHolds();
            Console.WriteLine("[PadOS] HoldSwitchHandler_OnTrigger");
            _timer.Start();
        }
    }
}
