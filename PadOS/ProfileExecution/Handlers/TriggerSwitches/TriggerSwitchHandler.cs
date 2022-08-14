using System;
using System.Collections.Generic;
using System.Linq;
using PadOS.Input.GamePadInput;
using PadOS.SaveData.ProfileXML;

namespace PadOS.ProfileExecution {
    public class TriggerSwitchHandler : ITriggerSwitchHandler {
        private bool _enabled;
        public bool Enabled {
            get { return _enabled; }
            set {
                _enabled = value;
                foreach (var item in _handlers) {
                    item.Enabled = value;
                }
            }
        }

        private List<ITriggerHandler> _handlers;
        private int _lastTrigger = -1;
        public event Action<ITriggerSwitchHandler, int> OnTrigger;
        private ButtonSequenceTriggerHandler _longenstSequence;

        // receive all events, and when timeout happens, trigger the longest one
        private void Handler_OnTimeout(ITriggerHandler sender) {
            foreach (var item in _handlers.OfType<ButtonSequenceTriggerHandler>()) {
                item.Reset();
            }
            if (_lastTrigger != -1) {
                var v = _lastTrigger;
                _lastTrigger = -1;
                OnTrigger?.Invoke(this, v);
            }
        }

        private void Handler_OnTrigger(ITriggerHandler trigger) {
            var seq = (ButtonSequenceTriggerHandler)trigger;
            _lastTrigger = _handlers.IndexOf(trigger);
            if (seq.SequenceLength == _longenstSequence.SequenceLength) {
                seq.Reset();
                Handler_OnTimeout(trigger);
            }
        }

        public void Init(ITrigger node, GamePadInput input) {
            var triggerSwitch = node as TriggerSwitch;
            _handlers = new List<ITriggerHandler>();
            foreach (var item in triggerSwitch.Triggers) {
                switch (item) {
                    case SequenceTrigger seq:
                        var handler = Maps.TriggerHandlers.CreateInstance<ITrigger, ButtonSequenceTriggerHandler>(seq);
                        handler.Init(seq, input);
                        handler.OnTrigger += Handler_OnTrigger;
                        handler.OnTimeout += Handler_OnTimeout;
                        _handlers.Add(handler);
                        if (_longenstSequence == null || handler.SequenceLength > _longenstSequence.SequenceLength) {
                            _longenstSequence = handler;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}