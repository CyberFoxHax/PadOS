using System;
using System.Collections;
using System.Collections.Generic;
using static XInputDotNetPure.GamePadState;
using GamePadInput = PadOS.Input.GamePadInput.GamePadInput;

namespace PadOS.ProfileExecution
{
    public static class Maps {
        public static TypedMap TriggerHandlers = new TypedMap {
            TypedMap.Typed<SaveData.ProfileXML.SequenceTrigger, SequenceTriggerHandler>(),
            TypedMap.Typed<SaveData.ProfileXML.ComboTrigger, ComboTriggerHandler>(),
            TypedMap.Typed<SaveData.ProfileXML.TriggerSwitch, TriggerSwitchHandler>(),
            TypedMap.Typed<SaveData.ProfileXML.PadOSTrigger, PadOSTriggerHandler>(),
            TypedMap.Typed<SaveData.ProfileXML.ButtonTrigger, ButtonTriggerHandler>(),
        };

        public static TypedMap ActionHandlers = new TypedMap {
            TypedMap.Typed<SaveData.ProfileXML.KeyboardAction, KeyboardActionHandler>(),
        };

        public static readonly Dictionary<ButtonsConstants, string> ButtonDownEventMap = new Dictionary<ButtonsConstants, string> {
            { ButtonsConstants.DPadUp, nameof(GamePadInput.DPadUpDown) },
            { ButtonsConstants.DPadDown, nameof(GamePadInput.DPadDownDown) },
            { ButtonsConstants.DPadLeft, nameof(GamePadInput.DPadLeftDown) },
            { ButtonsConstants.DPadRight, nameof(GamePadInput.DPadRightDown) },
            { ButtonsConstants.Start, nameof(GamePadInput.ButtonStartDown) },
            { ButtonsConstants.Back, nameof(GamePadInput.ButtonBackDown) },
            { ButtonsConstants.LeftThumb, nameof(GamePadInput.ButtonLeftStickDown) },
            { ButtonsConstants.RightThumb, nameof(GamePadInput.ButtonRightStickDown) },
            { ButtonsConstants.LeftShoulder, nameof(GamePadInput.ButtonLeftShoulderDown) },
            { ButtonsConstants.RightShoulder, nameof(GamePadInput.ButtonRightShoulderDown) },
            { ButtonsConstants.Guide, nameof(GamePadInput.ButtonGuideDown) },
            { ButtonsConstants.A, nameof(GamePadInput.ButtonADown) },
            { ButtonsConstants.B, nameof(GamePadInput.ButtonBDown) },
            { ButtonsConstants.X, nameof(GamePadInput.ButtonXDown) },
            { ButtonsConstants.Y, nameof(GamePadInput.ButtonYDown) }
        };

        public static readonly Dictionary<ButtonsConstants, string> ButtonUpEventMap = new Dictionary<ButtonsConstants, string> {
            { ButtonsConstants.DPadUp, nameof(GamePadInput.DPadUpUp) },
            { ButtonsConstants.DPadDown, nameof(GamePadInput.DPadUpUp) },
            { ButtonsConstants.DPadLeft, nameof(GamePadInput.DPadLeftUp) },
            { ButtonsConstants.DPadRight, nameof(GamePadInput.DPadRightUp) },
            { ButtonsConstants.Start, nameof(GamePadInput.ButtonStartUp) },
            { ButtonsConstants.Back, nameof(GamePadInput.ButtonBackUp) },
            { ButtonsConstants.LeftThumb, nameof(GamePadInput.ButtonLeftStickUp) },
            { ButtonsConstants.RightThumb, nameof(GamePadInput.ButtonRightStickUp) },
            { ButtonsConstants.LeftShoulder, nameof(GamePadInput.ButtonLeftShoulderUp) },
            { ButtonsConstants.RightShoulder, nameof(GamePadInput.ButtonRightShoulderUp) },
            { ButtonsConstants.Guide, nameof(GamePadInput.ButtonGuideUp) },
            { ButtonsConstants.A, nameof(GamePadInput.ButtonAUp) },
            { ButtonsConstants.B, nameof(GamePadInput.ButtonBUp) },
            { ButtonsConstants.X, nameof(GamePadInput.ButtonXUp) },
            { ButtonsConstants.Y, nameof(GamePadInput.ButtonYUp) }
        };

        public static ButtonsConstants StringToButton(string btn) {
            return (ButtonsConstants)System.Enum.Parse(typeof(ButtonsConstants), btn);
        }

        public class TypedMap : IEnumerable<KeyValuePair<Type, Type>>{
            public Dictionary<Type, Type> TriggerHandlers = new Dictionary<Type, Type>();

            public T2 GetInstance<T1, T2>(T1 instance) {
                var triggerType = instance.GetType();
                Type type;
                if (TriggerHandlers.TryGetValue(triggerType, out type) == false)
                    return default;
                return (T2)Activator.CreateInstance(type);
            }

            public void Add(KeyValuePair<Type, Type> p) {
                TriggerHandlers.Add(p.Key, p.Value);
            }

            public static KeyValuePair<Type, Type> Typed<T1, T2>() {
                return new KeyValuePair<Type, Type>(typeof(T1), typeof(T2));
            }

            public IEnumerator<KeyValuePair<Type, Type>> GetEnumerator() => null;
            IEnumerator IEnumerable.GetEnumerator() => null;
        }
    }
}
