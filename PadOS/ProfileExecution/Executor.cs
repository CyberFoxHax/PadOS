using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using PadOS.SaveData.ProfileXML;
using PadOS.Input.GamePadInput;
using static XInputDotNetPure.GamePadState;

namespace PadOS.ProfileExecution
{
    public class Executor {
        public Executor(Profile profile, GamePadInput gamePadInput) {
            _profile = profile;
            _gamePadInput = gamePadInput;
        }
        private GamePadInput _gamePadInput;
        private readonly Profile _profile;
        private List<InputSimulatorPlugin> _plugins;
        private List<ITriggerHandler> _triggers = new List<ITriggerHandler>();
        private List<ITriggerSwitchHandler> _triggersSwitches = new List<ITriggerSwitchHandler>();

        private List<IActionHandler> _actions = new List<IActionHandler>();

        private class MappingHandler {
            private List<IActionHandler> _actions = new List<IActionHandler>();

            public void Add(IActionHandler action) {
                _actions.Add(action);
            }

            public void Invoke() {
                foreach (var item in _actions) {
                    item.Invoke();
                }
            }
        }

        private class SwitchMappingHandler {
            private List<IActionHandler> _actions = new List<IActionHandler>();

            public void Add(IActionHandler action) {
                _actions.Add(action);
            }

            public void Invoke(int index) {
                _actions[index].Invoke();
            }
        }

        public void Init() {
            var plugins = Plugins.PluginsLoader.FindCorrectDll(_profile.Plugins.Select(p => p.Filename));
            _plugins = plugins.Select(p => Plugins.PluginsLoader.Load<InputSimulatorPlugin>(p).CreateInstance()).ToList();

            foreach (var mapping in _profile.Mappings) {
                MappingHandler mappingHandler = null;
                SwitchMappingHandler switchMapping = null;
                foreach (var trigger in mapping.Triggers) {
                    if (trigger is TriggerSwitch) {
                        if(switchMapping == null)
                            switchMapping = new SwitchMappingHandler();
                        var sw = Maps.TriggerHandlers.GetInstance<ITrigger, ITriggerSwitchHandler>(trigger);
                        sw.Init(trigger, _gamePadInput);
                        sw.OnTrigger += p => {
                            switchMapping.Invoke(p);
                        };
                        _triggersSwitches.Add(sw);
                    }
                    else {
                        if (mappingHandler == null)
                            mappingHandler = new MappingHandler();
                        var handler = Maps.TriggerHandlers.GetInstance<ITrigger, ITriggerHandler>(trigger);
                        handler.Init(trigger, _gamePadInput);
                        handler.OnTrigger += () => {
                            mappingHandler.Invoke();
                        };
                        _triggers.Add(handler);
                    }
                }
                foreach (var action in mapping.Actions) {
                    var handler = Maps.ActionHandlers.GetInstance<IAction, IActionHandler>(action);
                    handler.Init(action);
                    _actions.Add(handler);
                    if (switchMapping != null)
                        switchMapping.Add(handler);
                    else if (mappingHandler != null)
                        mappingHandler.Add(handler);
                }
            }
        }

        private bool _enabled;
        public bool Enabled {
            get => _enabled;
            set {
                if (_enabled == value)
                    return;
                _enabled = value;
                if(value)
                    OnActivate();
                else
                    OnDeactivate();
            }
        }

        private void OnActivate() {
            const bool v = true;
            foreach (var item in _plugins) 
                item.Enabled = v;
            foreach (var item in _triggers)
                item.Enabled = v;
            foreach (var item in _triggersSwitches)
                item.Enabled = v;
        }

        private void OnDeactivate() {
            const bool v = false;
            foreach (var item in _plugins)
                item.Enabled = v;
            foreach (var item in _triggers)
                item.Enabled = v;
            foreach (var item in _triggersSwitches)
                item.Enabled = v;
        }
    }
}
