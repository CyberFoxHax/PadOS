using System;
using System.Collections.Generic;
using System.Linq;
using PadOS.SaveData.ProfileXML;
using PadOS.Input.GamePadInput;
using static XInputDotNetPure.GamePadState;

namespace PadOS.ProfileExecution {
    public class SequenceTriggerHandler : ITriggerHandler {

        public SequenceTriggerHandler() {
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = false;
            _timer.Stop();
        }

        public void Init(ITrigger n, GamePadInput input) {
            var node = (SequenceTrigger)n;
            _buttonSequence = node.Sequence
                .OfType<ButtonTrigger>()
                .Select(p => Maps.StringToButton(p.Button))
                .ToArray();
            _input = input;
            _timeout = node.Timeout;
            _timer.Interval = _timeout;
        }

        public event Action OnTrigger;
        public event Action OnTriggerOff;
        public event Action OnTimeout;

        private bool _enabled;
        public bool Enabled {
            get => _enabled;
            set {
                if (_enabled == value)
                    return;
                _enabled = value;
                if (value)
                    Enable();
                else
                    Disable();
            }
        }

        private void Enable() {
            var type = _input.GetType();
            foreach (var item in Maps.ButtonDownEventMap) {
                var evt = type.GetEvent(item.Value);
                _dict[evt] = (a, b) => OnButton(item.Key);
                evt.AddEventHandler(_input, _dict[evt]);
            }
        }
        private void Disable() {
            foreach (var item in _dict) {
                item.Key.RemoveEventHandler(_input, item.Value);
            }
        }

        private Dictionary<System.Reflection.EventInfo, Input.GamePadEvent> _dict = new Dictionary<System.Reflection.EventInfo, Input.GamePadEvent>();
        private GamePadInput _input;
        private ButtonsConstants[] _buttonSequence;
        //private DateTime _lastButtonTime;
        private int _timeout;
        private int _currentPosition = 0;
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();

        public void Reset() {
            _currentPosition = 0;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            _currentPosition = 0;
            OnTimeout?.Invoke();
            _timer.Stop();
        }

        private void OnButton(ButtonsConstants btn) {
            _timer.Stop();
            _timer.Start();

            if (btn != _buttonSequence[_currentPosition]) {
                _currentPosition = 0;
                return;
            }
            _currentPosition++;


            if (_currentPosition == _buttonSequence.Length) {
                _currentPosition = 0;
                OnTrigger?.Invoke();
            }
        }
    }
}
