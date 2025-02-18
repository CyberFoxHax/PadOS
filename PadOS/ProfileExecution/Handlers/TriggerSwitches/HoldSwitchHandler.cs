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

        public event Action<EventData, ITriggerSwitchHandler, int> OnTrigger;
        public event Action<EventData, ITriggerSwitchHandler> OnTriggerOff;

        private ITriggerHandler[] _triggerHandlers;
        private float[] _timeouts;
        private DateTime _startTime;
        private DateTime _endTime;
        private Timer _timer = new Timer { AutoReset = false };
        private bool _on;
        private int _lastPlayer;

        private int _buttonsDownCount = 0;
        private GamePadInput _gamePad;

        private void VibrateHolds() {
            var timers = _timeouts.Where(p=>p>0).Select(p=>new Timer {
                Interval = p,
                AutoReset = false,
                Enabled = true
            });
            foreach (var timer in timers) {
                timer.Elapsed += delegate {
                    if(_on)
                        _gamePad.SetVibrationOnce(_lastPlayer, 1, 1, 150);
                    timer.Enabled = false;
                    timer.Dispose();
                };
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
            _timer.Stop();
            _on = false;
            _endTime = DateTime.Now;
            var diff = (_endTime - _startTime).TotalMilliseconds;

            _endTime = default;
            _startTime = default;
            var index = _timeouts.Length - 1;
            _buttonsDownCount = 0;
            var eventData = new EventData {
                PlayerIndex = _lastPlayer
            };
            OnTrigger?.Invoke(eventData, this, index);
            OnTriggerOff?.Invoke(eventData, this);
        }

        private void HoldSwitchHandler_OnTriggerOff(EventData evt) {
            if (evt.PlayerIndex != null)
                _lastPlayer = evt.PlayerIndex.Value;
            _buttonsDownCount--;
            if (_on == false || _buttonsDownCount > 0)
                return;
            _timer.Stop();
            _on = false;
            _endTime = DateTime.Now;


            // find the correct event to trigger based on hold time.
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
            _buttonsDownCount = 0;
            var eventData = new EventData {
                PlayerIndex = _lastPlayer
            };
            OnTrigger?.Invoke(evt, this, index);
            OnTriggerOff?.Invoke(evt, this);
        }

        private void HoldSwitchHandler_OnTrigger(EventData sender) {
            if(sender.PlayerIndex != null)
                _lastPlayer = sender.PlayerIndex.Value;
            _buttonsDownCount++;
            _startTime = DateTime.Now;
            _on = true;
            VibrateHolds();
            _timer.Start();
        }
    }
}
