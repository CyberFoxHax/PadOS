using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadOS.Input.GamePadInput;
using PadOS.SaveData.ProfileXML;
using XInputDotNetPure;
using static XInputDotNetPure.GamePadState;

namespace PadOS.ProfileExecution {
    class ButtonTriggerHandler : ITriggerHandler {
        private bool _enabled;
        public bool Enabled {
            get { return _enabled; }
            set {
                _enabled = value;
                if (value)
                    Enable();
                else 
                    Disable();
            }
        }


        public event TriggerEvent OnTrigger;
        public event TriggerEvent OnTriggerOff;

        private GamePadInput _input;
        private Action<Input.GamePadEvent> Down_AddMethod;
        private Action<Input.GamePadEvent> Down_RemoveMethod;
        private Action<Input.GamePadEvent> Up_AddMethod;
        private Action<Input.GamePadEvent> Up_RemoveMethod;
        private ButtonsConstants _button;

        public override string ToString() {
            return $"{nameof(ButtonTriggerHandler)} => {_button}";
        }

        public void Init(ITrigger node, GamePadInput input) {
            var buttonNode = (ButtonTrigger)node;
            _input = input;

            var key = Maps.StringToButton(buttonNode.Button);
            _button = key;

            var type = _input.GetType();
            {
                var eventName = Maps.ButtonDownEventMap[key];
                var evt = type.GetEvent(eventName);
                Down_AddMethod = (Action<Input.GamePadEvent>)Delegate.CreateDelegate(typeof(Action<Input.GamePadEvent>), _input, evt.AddMethod);
                Down_RemoveMethod = (Action<Input.GamePadEvent>)Delegate.CreateDelegate(typeof(Action<Input.GamePadEvent>), _input, evt.RemoveMethod);
            }
            {
                var eventName = Maps.ButtonUpEventMap[key];
                var evt = type.GetEvent(eventName);
                Up_AddMethod = (Action<Input.GamePadEvent>)Delegate.CreateDelegate(typeof(Action<Input.GamePadEvent>), _input, evt.AddMethod);
                Up_RemoveMethod = (Action<Input.GamePadEvent>)Delegate.CreateDelegate(typeof(Action<Input.GamePadEvent>), _input, evt.RemoveMethod);
            }
        }

        private void OnButton(int player, GamePadState state) {
            OnTrigger?.Invoke(this);
        }

        private void OnButtonUp(int player, GamePadState state) {
            OnTriggerOff?.Invoke(this);
        }

        private void Enable() {
            Down_AddMethod(OnButton);
            Up_AddMethod(OnButtonUp);
        }
        private void Disable() {
            Down_RemoveMethod(OnButton);
            Up_RemoveMethod(OnButtonUp);
        }
    }
}
