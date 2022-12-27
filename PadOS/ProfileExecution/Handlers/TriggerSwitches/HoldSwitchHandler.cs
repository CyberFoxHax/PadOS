﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using PadOS.Input.GamePadInput;
using PadOS.SaveData.ProfileXML;

namespace PadOS.ProfileExecution {
    public class HoldSwitchHandler : ITriggerSwitchHandler {
        private bool _enabled;
        public bool Enabled {
            get => _enabled;
            set {
                _enabled = value;
                foreach (var item in _triggerHandlers)
                    item.Enabled = value;
            }
        }

        public event Action<ITriggerSwitchHandler, int> OnTrigger;
        public event Action<ITriggerSwitchHandler> OnTriggerOff;

        private ITriggerHandler[] _triggerHandlers;
        private float[] _timeouts;
        private DateTime _startTime;
        private DateTime _endTime;
        private Timer _timer = new Timer { AutoReset = false };
        private bool _on;

        private int _buttonsDownCount = 0;

        public void Init(ITrigger node, GamePadInput input) {
            var sw = node as HoldSwitch;
            _triggerHandlers = new ITriggerHandler[sw.Buttons.Count];
            _timeouts = new float[_triggerHandlers.Length];
            for (int i = 0; i < sw.Buttons.Count; i++) {
                _triggerHandlers[i] = Maps.TriggerHandlers.InitHandler(sw.Buttons[i].Owner, input);
                _timeouts[i] = sw.Buttons[i].Timeout;
                _triggerHandlers[i].OnTrigger += HoldSwitchHandler_OnTrigger;
                _triggerHandlers[i].OnTriggerOff += HoldSwitchHandler_OnTriggerOff;
            }
            _timer.Interval = _timeouts.Last();
            _timer.Elapsed += timer_Elapsed;
            _timer.Stop();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e) {
            _timer.Stop();
            _on = false;
            _endTime = DateTime.Now;
            var diff = (_endTime - _startTime).TotalMilliseconds;

            _endTime = default;
            _startTime = default;
            var index = _triggerHandlers.Length - 1;
            OnTrigger?.Invoke(this, index);
        }

        private void HoldSwitchHandler_OnTriggerOff(ITriggerHandler sender) {
            _buttonsDownCount--;
            if (_buttonsDownCount == 0) {
                OnTriggerOff?.Invoke(this);
            }
            if (_on == false)
                return;
            _timer.Stop();
            _on = false;
            _endTime = DateTime.Now;

            var diff = (_endTime - _startTime).TotalMilliseconds;
            var index = 0;
            float t = 0;
            if (diff > _timeouts[1]) {
                for (int i = _timeouts.Length - 1; i >= 0; i--)
                    if (diff > _timeouts[i]) {
                        index = i;
                        break;
                    }
            }


            _endTime = default;
            _startTime = default;
            OnTrigger?.Invoke(this, index);
        }

        private void HoldSwitchHandler_OnTrigger(ITriggerHandler sender) {
            _buttonsDownCount++;
            _startTime = DateTime.Now;
            _on = true;
            _timer.Start();
        }
    }
}
