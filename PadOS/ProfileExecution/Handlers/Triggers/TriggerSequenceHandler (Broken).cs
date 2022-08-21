using System;
using System.Collections.Generic;
using System.Linq;
using PadOS.SaveData.ProfileXML;
using PadOS.Input.GamePadInput;
using static XInputDotNetPure.GamePadState;

namespace PadOS.ProfileExecution {
    // wont detect incorrect sequences cause those events are not registered...
    public class __TriggerSequenceHandler : ITriggerHandler {

        public __TriggerSequenceHandler() {
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = false;
            _timer.Stop();
        }

        public void Init(ITrigger n, GamePadInput input) {
            var node = (SequenceTrigger)n;
            _sequence = new ITriggerHandler[node.Sequence.Count];
            for (int i = 0; i < node.Sequence.Count; i++) {
                var triggerNode = node.Sequence[i];
                var handler = Maps.TriggerHandlers.InstanceFromNode(triggerNode);
                handler.Init(triggerNode, input);
                _sequence[i] = handler;
            }
            _input = input;
            _timeout = node.Timeout;
            _timer.Interval = _timeout;
        }

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
            foreach (var handler in _sequence) {
                handler.OnTrigger += OnTriggerHandler;
                handler.Enabled = true;
            }
        }
        private void Disable() {
            foreach (var handler in _sequence) {
                handler.OnTrigger -= OnTriggerHandler;
                handler.Enabled = false;
            }
        }


        private void OnTriggerHandler(ITriggerHandler sender) {
            _timer.Stop();
            _timer.Start();

            if (sender != _sequence[_currentPosition]) {
                _currentPosition = 0;
                return;
            }
            _currentPosition++;

            if (_currentPosition == _sequence.Length) {
                _currentPosition = 0;
                OnTrigger?.Invoke(this);
                OnTriggerOff?.Invoke(this);
            }
        }


        private GamePadInput _input;
        private ITriggerHandler[] _sequence;
        //private DateTime _lastButtonTime;
        private int _timeout;
        private int _currentPosition = 0;
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();

        public void Reset() {
            _currentPosition = 0;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            _currentPosition = 0;
            OnTimeout?.Invoke(this);
            _timer.Stop();
        }
    }
}
