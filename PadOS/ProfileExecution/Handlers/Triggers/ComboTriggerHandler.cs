using System;
using System.Collections.Generic;
using System.Linq;
using PadOS.SaveData.ProfileXML;
using PadOS.Input.GamePadInput;
using static XInputDotNetPure.GamePadState;

namespace PadOS.ProfileExecution {
    public class ComboTriggerHandler : ITriggerHandler {
        public void Init(ITrigger n, GamePadInput input) {
            var node = (ComboTrigger)n;
            _buttonSequence = node.Sequence
                .OfType<ButtonTrigger>()
                .Select(p => Maps.StringToButton(p.Button))
                .ToArray();
            _input = input;
            _timeout = node.Timeout;
        }

        public event Action OnTrigger;
        public event Action OnTriggerOff;

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
                _dict[evt] = (a, b) => OnButton(item.Key, true);
                evt.AddEventHandler(_input, _dict[evt]);
            }
            foreach (var item in Maps.ButtonUpEventMap) {
                var evt = type.GetEvent(item.Value);
                _dict[evt] = (a, b) => OnButton(item.Key, false);
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
        private int _timeout;
        private int _comboCount = 0;
        private bool _awaitRelease = false;

        public void Reset() {
            _comboCount = 0;
        }
        private void OnButton(ButtonsConstants btn, bool down) {
            if (down) {
                // wrong equation, somewhat
                if (_buttonSequence.Contains(btn))
                    _comboCount++;
                else
                    _comboCount--;
            }
            else {
                if (_buttonSequence.Contains(btn))
                    _comboCount--;
                else
                    _comboCount++;
            }

            if (_awaitRelease == false && _comboCount == _buttonSequence.Length) {
                _awaitRelease = true;
                OnTrigger?.Invoke();
            }
            if (_comboCount == 0)
                _awaitRelease = false;

            //Console.WriteLine("InputNumber: " + _comboCount + " " + btn);
        }
    }
}
