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

        private AnalogueTrigger.EAxis _axis;
        private GamePadInput _input;
        private float _value;
        private System.Timers.Timer _timer = new System.Timers.Timer { AutoReset = true };
        private bool _triggerOn = false;
        private bool _isNegative = false;

        public void Init(ITrigger node, GamePadInput input) {
            var anal = (AnalogueTrigger)node;
            _input = input;
            _axis = anal.Axis;
            _value = anal.Value;
            _timer.Interval = anal.Frequency;
            _isNegative = _value < 0;
        }

        private void OnTimer(object sender, System.Timers.ElapsedEventArgs e) {
            OnTrigger?.Invoke(this);
        }

        private void OnThumbChange(int player, GamePadState state, Input.Vector2 vec2) {
            float value;
            switch (_axis) {
                case AnalogueTrigger.EAxis.RightThumbX:
                case AnalogueTrigger.EAxis.LeftThumbX:
                    value = (float)vec2.X;
                    break;
                case AnalogueTrigger.EAxis.RightThumbY:
                case AnalogueTrigger.EAxis.LeftThumbY:
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
                OnTrigger?.Invoke(this);
                OnTriggerOff?.Invoke(this);
            }
        }

        private void Activate() {
            if(_timer.Interval > 0)
                _timer.Elapsed += OnTimer;
            switch (_axis) {
                case AnalogueTrigger.EAxis.RightThumbX:
                case AnalogueTrigger.EAxis.RightThumbY:
                    _input.ThumbRightChange += OnThumbChange;
                    break;
                case AnalogueTrigger.EAxis.LeftThumbX:
                case AnalogueTrigger.EAxis.LeftThumbY:
                    _input.ThumbLeftChange += OnThumbChange;
                    break;
                case AnalogueTrigger.EAxis.RightTrigger:
                    _input.TriggerRightChange += OnTriggerChange;
                    break;
                case AnalogueTrigger.EAxis.LeftTrigger:
                    _input.TriggerLeftChange += OnTriggerChange;
                    break;
            }
        }

        private void Deactivate() {
            if (_timer.Interval > 0)
                _timer.Stop();
            switch (_axis) {
                case AnalogueTrigger.EAxis.RightThumbX:
                case AnalogueTrigger.EAxis.RightThumbY:
                    _input.ThumbRightChange -= OnThumbChange;
                    break;
                case AnalogueTrigger.EAxis.LeftThumbX:
                case AnalogueTrigger.EAxis.LeftThumbY:
                    _input.ThumbLeftChange -= OnThumbChange;
                    break;
                case AnalogueTrigger.EAxis.RightTrigger:
                    _input.TriggerRightChange -= OnTriggerChange;
                    break;
                case AnalogueTrigger.EAxis.LeftTrigger:
                    _input.TriggerLeftChange -= OnTriggerChange;
                    break;
            }
        }
    }
}
