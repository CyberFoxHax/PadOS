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
        private readonly List<ITriggerHandler> _triggers = new List<ITriggerHandler>();
        private readonly List<ITriggerSwitchHandler> _triggersSwitches = new List<ITriggerSwitchHandler>();

        private List<IActionHandler> _actions = new List<IActionHandler>();
        private List<MappingHandler> _mappingHandlers = new List<MappingHandler>();
        private List<SwitchMappingHandler> _switchMappingHandlers = new List<SwitchMappingHandler>();


        public void Init() {
            var plugins = Plugins.PluginsLoader.FindCorrectDll(_profile.Plugins.Select(p => p.Filename));
            _plugins = plugins.Select(p => Plugins.PluginsLoader.Load<InputSimulatorPlugin>(p).CreateInstance()).ToList();

            foreach (var mapping in _profile.Mappings) {
                MappingHandler mappingHandler = null;
                SwitchMappingHandler switchMapping = null;
                foreach (var trigger in mapping.Triggers) {
                    if (trigger is TriggerSwitch || trigger is HoldSwitch) {
                        if(switchMapping == null)
                            switchMapping = new SwitchMappingHandler();
                        var sw = Maps.TriggerSwitchHandlers.InstanceFromNode(trigger);
                        sw.Init(trigger, _gamePadInput);
                        sw.OnTrigger += (s, i) => {
                            if (_awaitKeysUpTask == null)
                                switchMapping.Invoke(i);
                        };
                        sw.OnTriggerOff += (s) => {
                            switchMapping.InvokeOff();
                            if (_awaitKeysUpTask != null)
                                CheckAwaitKeysUp();
                        };
                        _triggersSwitches.Add(sw);
                    }
                    else {
                        if (mappingHandler == null)
                            mappingHandler = new MappingHandler();
                        var handler = Maps.TriggerHandlers.InstanceFromNode(trigger);
                        handler.Init(trigger, _gamePadInput);
                        handler.OnTrigger += p => {
                            if (_awaitKeysUpTask == null)
                                mappingHandler.Invoke();
                        };
                        handler.OnTriggerOff += p => {
                            mappingHandler.InvokeOff();
                            if (_awaitKeysUpTask != null)
                                CheckAwaitKeysUp();
                        };
                        _triggers.Add(handler);
                    }
                }
                foreach (var action in mapping.Actions) {
                    var handler = Maps.ActionHandlers.InstanceFromNode(action);
                    handler.Init(action);
                    _actions.Add(handler);
                    if (switchMapping != null)
                        switchMapping.Add(handler);
                    else if (mappingHandler != null)
                        mappingHandler.Add(handler);
                }
                if (mappingHandler != null)
                    _mappingHandlers.Add(mappingHandler);
                else if (switchMapping != null)
                    _switchMappingHandlers.Add(switchMapping);
            }
        }

        private void CheckAwaitKeysUp() {
            if (_mappingHandlers.All(p => p.AnyDown == false)
            && _switchMappingHandlers.All(p=>p.AnyDown == false)) {
                var t = _awaitKeysUpTask;
                _awaitKeysUpTask = null;
                t.SetResult(true);
            }
        }

        private TaskCompletionSource<bool> _awaitKeysUpTask;
        public Task AwaitAllKeysUp() {
            if (_mappingHandlers.All(p => p.AnyDown == false)
            && _switchMappingHandlers.All(p => p.AnyDown == false)
            && _awaitKeysUpTask == null) {
                Console.WriteLine("Dont wait");
                return Task.FromResult(true);
            }

            if (_awaitKeysUpTask != null)
                return _awaitKeysUpTask.Task;
            _awaitKeysUpTask = new TaskCompletionSource<bool>();
            Console.WriteLine("Wait");
            return _awaitKeysUpTask.Task;
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
            foreach (var item in _actions)
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
            foreach (var item in _actions)
                item.Enabled = v;
        }
    }
}
