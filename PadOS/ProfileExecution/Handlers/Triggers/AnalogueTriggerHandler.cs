using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadOS.Input.GamePadInput;
using PadOS.SaveData.ProfileXML;
using XInputDotNetPure;

namespace PadOS.ProfileExecution {
    public class AnalogueTriggerHandler : ITriggerHandler {
        private bool _enabled;
        public bool Enabled {
            get => _enabled;
            set {
                _enabled = value;
                if (value)
                    Activate();
                else
                    Deactivate();
            }
        }

        public event TriggerEvent OnTrigger;
        public event TriggerEvent OnTriggerOff;

        private GamePadInput.Axis _axis;
        private GamePadInput _input;
        private float _value;
        private System.Timers.Timer _timer = new System.Timers.Timer { AutoReset = true };
        private bool _triggerOn = false;
        private bool _isNegative = false;
        private int _lastPlayer;

        public void Init(ITrigger node, GamePadInput input) {
            var anal = (AnalogueTrigger)node;
            _input = input;
            _axis = anal.Axis;
            _value = anal.Value;
            _timer.Interval = anal.Frequency;
            _isNegative = _value < 0;
        }

        private void OnTimer(object sender, System.Timers.ElapsedEventArgs e) {
            var evt = new EventData {
                Sender = this,
                PlayerIndex = _lastPlayer
            };
            OnTrigger?.Invoke(evt);
            OnTriggerOff?.Invoke(evt);
        }

        private void OnThumbChange(int player, GamePadState state, Input.Vector2 vec2) {
            float value;
            switch (_axis) {
                case GamePadInput.Axis.RightThumbX:
                case GamePadInput.Axis.LeftThumbX:
                    value = (float)vec2.X;
                    break;
                case GamePadInput.Axis.RightThumbY:
                case GamePadInput.Axis.LeftThumbY:
                    value = (float)vec2.Y;
                    break;
                default:
                    return;
            }
            if (_isNegative) {
                value = -value;
            }
            OnTriggerChange(player, state, value);
        }

        private void OnTriggerChange(int player, GamePadState state, float value) {
            _lastPlayer = player;
            //if(value > 0)
            //    _timer.Interval = freq * 1/value;
            var thresh = _value;
            if (_isNegative)
                thresh = -thresh;
            if (value < thresh) {
                _timer.Stop();
                _triggerOn = false;
            }
            else if (_triggerOn == false) { 
                _triggerOn = true;
                _timer.Start();
                var eventData = new EventData {
                    Sender = this,
                    PlayerIndex = player,
                    Axis = _axis,
                    AnalogueValue = value,
                };
                OnTrigger?.Invoke(eventData);
                OnTriggerOff?.Invoke(eventData);
            }
        }

        private void Activate() {
            if(_timer.Interval > 0)
                _timer.Elapsed += OnTimer;
            switch (_axis) {
                case GamePadInput.Axis.RightThumbX:
                case GamePadInput.Axis.RightThumbY:
                    _input.ThumbRightChange += OnThumbChange;
                    break;
                case GamePadInput.Axis.LeftThumbX:
                case GamePadInput.Axis.LeftThumbY:
                    _input.ThumbLeftChange += OnThumbChange;
                    break;
                case GamePadInput.Axis.RightTrigger:
                    _input.TriggerRightChange += OnTriggerChange;
                    break;
                case GamePadInput.Axis.LeftTrigger:
                    _input.TriggerLeftChange += OnTriggerChange;
                    break;
            }
        }

        private void Deactivate() {
            if (_timer.Interval > 0)
                _timer.Stop();
            switch (_axis) {
                case GamePadInput.Axis.RightThumbX:
                case GamePadInput.Axis.RightThumbY:
                    _input.ThumbRightChange -= OnThumbChange;
                    break;
                case GamePadInput.Axis.LeftThumbX:
                case GamePadInput.Axis.LeftThumbY:
                    _input.ThumbLeftChange -= OnThumbChange;
                    break;
                case GamePadInput.Axis.RightTrigger:
                    _input.TriggerRightChange -= OnTriggerChange;
                    break;
                case GamePadInput.Axis.LeftTrigger:
                    _input.TriggerLeftChange -= OnTriggerChange;
                    break;
            }
        }
    }
}
