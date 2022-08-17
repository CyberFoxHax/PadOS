using System;
using System.Collections;
using System.Collections.Generic;
using static XInputDotNetPure.GamePadState;
using GamePadInput = PadOS.Input.GamePadInput.GamePadInput;

namespace PadOS.ProfileExecution
{
    public static class Maps {
        public class TypeMap<T1, T2> : Dictionary<Type, Type> {
            public static KeyValuePair<Type, Type> Pair<TT1, TT2>() where TT1:T1 where TT2:T2{
                return new KeyValuePair<Type, Type>(typeof(TT1), typeof(TT2));
            }

            public void Add(KeyValuePair<Type, Type> p) {
                base.Add(p.Key, p.Value);
            }

            public T2 InstanceFromNode(T1 instance) {
                var triggerType = instance.GetType();
                Type type;
                if (TryGetValue(triggerType, out type) == false)
                    return default;
                return (T2)Activator.CreateInstance(type);
            }

            public T2 InitHandler(SaveData.ProfileXML.ITrigger node, GamePadInput input) {
                var instance = InstanceFromNode((T1)node);
                if(instance is ITriggerInit init)
                    init.Init(node, input);
                return instance;
            }
        }

        public class TriggerMap : TypeMap<SaveData.ProfileXML.ITrigger, ITriggerHandler> { }
        public class TriggerSwitchMap : TypeMap<SaveData.ProfileXML.ITrigger, ITriggerSwitchHandler> { }
        public class ActionMap : TypeMap<SaveData.ProfileXML.IAction, IActionHandler> { }

        public static TriggerMap derp = new TriggerMap {
            TriggerMap.Pair<SaveData.ProfileXML.ButtonTrigger, ButtonTriggerHandler>()
        };

        public static TriggerMap TriggerHandlers = new TriggerMap {
            TriggerMap.Pair<SaveData.ProfileXML.SequenceTrigger, ButtonSequenceTriggerHandler>(),
            TriggerMap.Pair<SaveData.ProfileXML.ComboTrigger, ComboTriggerHandler>(),
            TriggerMap.Pair<SaveData.ProfileXML.PadOSTrigger, PadOSTriggerHandler>(),
            TriggerMap.Pair<SaveData.ProfileXML.ButtonTrigger, ButtonTriggerHandler>(),
            TriggerMap.Pair<SaveData.ProfileXML.AnalogueTrigger, AnalogueTriggerHandler>(),
        };

        public static TriggerSwitchMap TriggerSwitchHandlers = new TriggerSwitchMap {
            TriggerSwitchMap.Pair<SaveData.ProfileXML.TriggerSwitch, TriggerSequenceSwitchHandler>(),
            TriggerSwitchMap.Pair<SaveData.ProfileXML.HoldSwitch, HoldSwitchHandler>(),
        };

        public static ActionMap ActionHandlers = new ActionMap {
            ActionMap.Pair<SaveData.ProfileXML.KeyboardAction, KeyboardActionHandler>(),
            ActionMap.Pair<SaveData.ProfileXML.MouseAction, MouseActionHandler>(),
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

        /*public class TypedMap : IEnumerable<KeyValuePair<Type, Type>>{
            public Dictionary<Type, Type> TriggerHandlers = new Dictionary<Type, Type>();

            public T2 CreateInstance<T1, T2>(T1 instance) {
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
        }*/
    }
}
