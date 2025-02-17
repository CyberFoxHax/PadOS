using System;
using System.Timers;

namespace PadOS.Views.GamePadOSK
{
    public class RepeatTimer {
        public RepeatTimer() {
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.systeminformation.keyboardspeed
            var interval = System.Windows.Forms.SystemInformation.KeyboardSpeed;
            var repeatsPerSecond = 2.5f + (30 - 2.5f) / 31 * interval;
            _keyboardInterval = 1000f / repeatsPerSecond;

            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.systeminformation.keyboarddelay
            var delay = System.Windows.Forms.SystemInformation.KeyboardDelay;
            _keyboardDelay = 250 + (1000 - 250) / 3 * delay;

            _repeatTimer = new Timer { AutoReset = true };
            _repeatTimer.Elapsed += Elapsed;
        }

        public Action Callback;

        private readonly float _keyboardInterval;
        private readonly int _keyboardDelay;
        private readonly Timer _repeatTimer;
        private bool _once = false;

        public void Start() {
            _once = false;
            _repeatTimer.Interval = _keyboardDelay;
            _repeatTimer.Start();
        }

        public void Stop() {
            _once = false;
            _repeatTimer.Stop();
        }

        private void Elapsed(object sender, ElapsedEventArgs e) {
            if (_once == false) {
                _once = true;
                _repeatTimer.Interval = _keyboardInterval;
            }
            Callback?.Invoke();
        }
    }
}
