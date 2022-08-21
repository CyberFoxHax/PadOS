using System;
using System.Collections.Generic;
using System.Linq;
using PadOS.SaveData.ProfileXML;
using PadOS.Input.GamePadInput;
using static XInputDotNetPure.GamePadState;

namespace PadOS.ProfileExecution {
    public class ButtonSequenceTriggerHandler : ITriggerHandler {

        public ButtonSequenceTriggerHandler() {
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


        public int SequenceLength => _buttonSequence.Length;
        public event TriggerEvent OnTrigger;
        public event TriggerEvent OnTriggerOff;
        public event TriggerEvent OnTimeout;

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
            _input.ButtonDown += OnButton;
        }
        private void Disable() {
            _input.ButtonDown -= OnButton;
        }

        private GamePadInput _input;
        private ButtonsConstants[] _buttonSequence;
        private int _timeout;
        private int _currentPosition = 0;
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();

        public void Reset() {
            _currentPosition = 0;
            _timer.Stop();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            _currentPosition = 0;
            OnTimeout?.Invoke(this);
            _timer.Stop();
        }

        private void OnButton(ButtonsConstants btn, int player, XInputDotNetPure.GamePadState state) {
            _timer.Stop();
            _timer.Start();

            if (btn != _buttonSequence[_currentPosition]) {
                _currentPosition = 0;
                return;
            }
            _currentPosition++;

            if (_currentPosition == _buttonSequence.Length) {
                _currentPosition = 0;
                OnTrigger?.Invoke(this);
                OnTriggerOff?.Invoke(this);
            }
        }
    }
}
