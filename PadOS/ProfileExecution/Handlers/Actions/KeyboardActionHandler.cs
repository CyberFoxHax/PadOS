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
        private int[] _vkSequence;

        private const string SupportedCharacters = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm0123456789";
        private static readonly string[] SupportedKeys = {
            "Shift","Ctrl","Alt","Tab",
            "Up","Down","Left","Right",
            "Delete","Backspace","Enter",
            "Escape","Space","Win"
        };

        public void Init(SaveData.ProfileXML.IAction node) {
            var keyboard = (SaveData.ProfileXML.KeyboardAction)node;
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

        public void Invoke() {
            if (_enabled)
                for (int i = 0; i < _vkSequence.Length; i++)
                    DllImport.UserInfo32.keybd_event(_vkSequence[i], 0, DllImport.UserInfo32.KEYEVENTF_KEYDOWN, 0);
        }

        public void InvokeOff() {
            if (_enabled)
                for (int i = _vkSequence.Length - 1; i >= 0; i--)
                    DllImport.UserInfo32.keybd_event(_vkSequence[i], 0, DllImport.UserInfo32.KEYEVENTF_KEYUP, 0);
        }
    }
}
