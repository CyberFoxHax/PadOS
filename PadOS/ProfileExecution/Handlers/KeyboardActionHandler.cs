using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadOS.ProfileExecution {
    public class KeyboardActionHandler : IActionHandler {

        private bool _enabled;
        public bool Enabled {
            get => _enabled;
            set {
                _enabled = value;
            }
        }

        private List<string> _buttons;

        public void Init(SaveData.ProfileXML.IAction node) {
            var keyboard = (SaveData.ProfileXML.KeyboardAction)node;
            _buttons = keyboard.Buttons;
        }

        public void Invoke() {
            // doesn't handle capital letters
            var seq = new int[_buttons.Count];
            for (int i = 0; i < _buttons.Count; i++) {
                string btn = _buttons[i];
                switch (btn) {
                    case "Shift"    : seq[i] = DllImport.UserInfo32.VK_LSHIFT; continue;
                    case "Ctrl"     : seq[i] = DllImport.UserInfo32.VK_LCONTROL; continue;
                    case "Alt"      : seq[i] = DllImport.UserInfo32.VK_MENU; continue;
                    case "Tab"      : seq[i] = DllImport.UserInfo32.VK_TAB; continue;
                    case "Up"       : seq[i] = DllImport.UserInfo32.VK_UP; continue;
                    case "Down"     : seq[i] = DllImport.UserInfo32.VK_DOWN; continue;
                    case "Left"     : seq[i] = DllImport.UserInfo32.VK_LEFT; continue;
                    case "Right"    : seq[i] = DllImport.UserInfo32.VK_RIGHT; continue;
                    case "Delete"   : seq[i] = DllImport.UserInfo32.VK_DELETE; continue;
                    case "Backspace": seq[i] = DllImport.UserInfo32.VK_BACK; continue;
                    case "Enter"    : seq[i] = DllImport.UserInfo32.VK_RETURN; continue;
                    case "Escape"   : seq[i] = DllImport.UserInfo32.VK_ESCAPE; continue;
                    case "Space"    : seq[i] = DllImport.UserInfo32.VK_SPACE; continue;
                }
                if (btn[0] == 'F' && btn.Length > 1) {
                    var vkfstart = 0x70-1;
                    var fnum = int.Parse(btn.Substring(1));
                    if (fnum > 24)
                        continue;
                    seq[i] = vkfstart + fnum;
                    continue;
                }
                seq[i] = btn[0];
            }

            for (int i = 0; i < seq.Length; i++)
                DllImport.UserInfo32.keybd_event(seq[i], 0, DllImport.UserInfo32.KEYEVENTF_KEYDOWN, 0);

            for (int i = seq.Length- 1; i >= 0; i--)
                DllImport.UserInfo32.keybd_event(seq[i], 0, DllImport.UserInfo32.KEYEVENTF_KEYUP, 0);
        }
    }
}
