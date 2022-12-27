using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

// https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
// https://www.rapidtables.com/code/text/ascii-table.html
namespace PadOS.ProfileExecution {
    public class KeyboardActionHandler : IActionHandler {

        private static readonly float KeyboardInterval;
        private static readonly float KeyboardDelay;
        static KeyboardActionHandler(){
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.systeminformation.keyboardspeed
            var interval = System.Windows.Forms.SystemInformation.KeyboardSpeed;
            var repeatsPerSecond = 2.5f + (30-2.5f)/31 * interval;
            KeyboardInterval = 1000f/repeatsPerSecond;

            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.systeminformation.keyboarddelay
            var delay = System.Windows.Forms.SystemInformation.KeyboardDelay;
            KeyboardDelay = 250 + (1000-250)/3 * delay;
        }

        private bool _enabled;
        public bool Enabled {
            get => _enabled;
            set {
                _enabled = value;
                if (_repeat) {
                    _delayTimer.Stop();
                    _intervalTimer.Stop();
                }
                // prevent getting stuck in downstate when switching profiles
                if (value == false && _isDown) {
                    KeysUp();
                }
            }
        }

        private List<string> _buttons;
        private int[] _vkSequence;

        private Timer _delayTimer;
        private Timer _intervalTimer;
        private bool _repeat = false;
        private bool _isDown = false;

        private const string SupportedCharacters = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm0123456789";
        private static readonly string[] SupportedKeys = {
            "Shift","Ctrl","Alt","Tab",
            "Up","Down","Left","Right",
            "Delete","Backspace","Enter",
            "Escape","Space","Win"
        };

        public void Init(SaveData.ProfileXML.IAction node) {
            var keyboard = (SaveData.ProfileXML.KeyboardAction)node;
            _repeat = keyboard.Repeat;
            if (keyboard.Repeat) {
                _delayTimer = new Timer { AutoReset = false, Interval = KeyboardDelay };
                _intervalTimer = new Timer { AutoReset = true, Interval = KeyboardInterval };
                _delayTimer.Elapsed += delayTimer_Elapsed;
                _intervalTimer.Elapsed += intervalTimer_Elapsed;
            }
            _buttons = keyboard.Buttons
                .Select(p=>p.Trim())
                .Where(
                    p=>(p.Length==1 && SupportedCharacters.Contains(p[0]))
                    || (p.Length>1 && SupportedKeys.Contains(p))
                    || p.Length==2 && p[0]=='F'
                ).ToList();

            var seq = new int[_buttons.Count];
            for (int i=0,ii=0;ii<_buttons.Count;i++,ii++) {
                string btn = _buttons[ii];
                switch (btn) {
                    case "Shift": seq[i] = DllImport.UserInfo32.VK_LSHIFT; continue;
                    case "Ctrl": seq[i] = DllImport.UserInfo32.VK_LCONTROL; continue;
                    case "Alt": seq[i] = DllImport.UserInfo32.VK_MENU; continue;
                    case "Tab": seq[i] = DllImport.UserInfo32.VK_TAB; continue;
                    case "Up": seq[i] = DllImport.UserInfo32.VK_UP; continue;
                    case "Down": seq[i] = DllImport.UserInfo32.VK_DOWN; continue;
                    case "Left": seq[i] = DllImport.UserInfo32.VK_LEFT; continue;
                    case "Right": seq[i] = DllImport.UserInfo32.VK_RIGHT; continue;
                    case "Delete": seq[i] = DllImport.UserInfo32.VK_DELETE; continue;
                    case "Backspace": seq[i] = DllImport.UserInfo32.VK_BACK; continue;
                    case "Enter": seq[i] = DllImport.UserInfo32.VK_RETURN; continue;
                    case "Escape": seq[i] = DllImport.UserInfo32.VK_ESCAPE; continue;
                    case "Space": seq[i] = DllImport.UserInfo32.VK_SPACE; continue;
                    case "Win": seq[i] = DllImport.UserInfo32.VK_LWIN; continue;
                }
                if (btn[0] == 'F' && btn.Length > 1) {
                    var fnum = int.Parse(btn.Substring(1));
                    if (fnum > 24)
                        continue;
                    seq[i] = 0x6F + fnum;
                    continue;
                }
                // VK numbers 0x30-0x39
                // VK lowercase 0x41-0x5A
                // ASCII numbers 0x30-0x39
                // ASCII lowercase 0x61-0x7A
                // ASCII uppercase 0x41-0x5A

                if (btn[0] >= 'A' && btn[0] <= 'Z') {
                    seq[i] = btn[0];
                }
                else if (btn[0] >= 'a' && btn[0] <= 'z') {
                    seq[i] = btn[0] - (0x61-0x41);
                }
                else if (btn[0] >= '0' && btn[0] <= '9') {
                    seq[i] = btn[0];
                }
            }
            _vkSequence = seq.ToArray();
        }

        private void intervalTimer_Elapsed(object sender, ElapsedEventArgs e) {
            KeysDown();
        }

        private void delayTimer_Elapsed(object sender, ElapsedEventArgs e) {
            _delayTimer.Stop();
            _intervalTimer.Start();
            KeysDown();
        }

        public void Invoke() {
            if (_enabled == false)
                return;
            KeysDown();
            if (_repeat) {
                _delayTimer.Start();
                _intervalTimer.Stop();
            }
        }

        public void InvokeOff() {
            if (_enabled == false)
                return;
            KeysUp();
            if (_repeat) {
                _delayTimer.Stop();
                _intervalTimer.Stop();
            }
        }

        private void KeysDown() {
            _isDown = true;
            var trout = "";
            for (int i = 0; i < _vkSequence.Length; i++)
                trout += _vkSequence[i]+" ";
            Console.WriteLine(trout);
            for (int i = 0; i < _vkSequence.Length; i++) {
                uint tf = DllImport.UserInfo32.KEYEVENTF_KEYDOWN;
                //if (_vkSequence[i] == DllImport.UserInfo32.VK_LSHIFT)
                //    tf |= DllImport.UserInfo32.KEYEVENTF_EXTENDEDKEY;
                DllImport.UserInfo32.keybd_event(_vkSequence[i], 0, tf, 0);
            }
        }

        private void KeysUp() {
            _isDown = false;
            var trout = "";
            for (int i = _vkSequence.Length - 1; i >= 0; i--)
                trout += _vkSequence[i]+" ";
            Console.WriteLine(trout);
            for (int i = _vkSequence.Length - 1; i >= 0; i--) {
                uint tf = DllImport.UserInfo32.KEYEVENTF_KEYUP;
                //if (_vkSequence[i] == DllImport.UserInfo32.VK_LSHIFT)
                //    tf |= DllImport.UserInfo32.KEYEVENTF_EXTENDEDKEY;
                DllImport.UserInfo32.keybd_event(_vkSequence[i], 0, tf, 0);
            }
        }
    }
}
