using System;
using PadOS.SaveData.ProfileXML;

/*
<MouseAction Button="Left" Position="100 100" />
<MouseAction Scroll="Y" Speed="20" />
<MouseAction Move="X" Speed="15" />
<MouseAction Position="100 100" />
<MouseAction Button="Left" />
*/
namespace PadOS.ProfileExecution {
    public class MouseActionHandler : IActionHandler {
        public bool Enabled { get; set; }

        private MouseAction.EButton _button;
        private MouseAction.EAxis _scrollAxis;
        //private MouseAction.EAxis _moveAxis;
        private float _speed;
        private MouseAction.Vector2 _setPosition;

        public void Init(IAction _) {
            var node = (MouseAction)_;
            _button = node.Button;
            _scrollAxis = node.Scroll;
            //_moveAxis = node.Move;
            _speed = node.Speed;
            _setPosition = node.Position;
        }

        public void Invoke() {
            uint vk = 0;
            int x = 0;
            switch (_button) {
                case MouseAction.EButton.Left:
                    vk = DllImport.UserInfo32.MOUSEEVENTF_LEFTDOWN;
                    break;
                case MouseAction.EButton.Middle:
                    vk = DllImport.UserInfo32.MOUSEEVENTF_MIDDLEDOWN;
                    break;
                case MouseAction.EButton.Right:
                    vk = DllImport.UserInfo32.MOUSEEVENTF_RIGHTDOWN;
                    break;
                case MouseAction.EButton.Forward:
                    vk = DllImport.UserInfo32.MOUSEEVENTF_XDOWN;
                    x = DllImport.UserInfo32.XBUTTON2;
                    break;
                case MouseAction.EButton.Back:
                    vk = DllImport.UserInfo32.MOUSEEVENTF_XDOWN;
                    x = DllImport.UserInfo32.XBUTTON1;
                    break;
            }
            if (_setPosition.IsNaN() == false) {
                DllImport.UserInfo32.POINT point = new DllImport.UserInfo32.POINT();

                var primaryScreenBounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                if (_setPosition.XUnit == MouseAction.EUnit.Percentage)
                    point.x = (int)(primaryScreenBounds.Width * _setPosition.X/100f);
                else
                    point.x = (int)_setPosition.X;

                if (_setPosition.YUnit == MouseAction.EUnit.Percentage)
                    point.y = (int)(primaryScreenBounds.Height * _setPosition.Y/100f);
                else
                    point.y = (int)_setPosition.Y;

                DllImport.UserInfo32.SetCursorPos(point);
            }
            if (_button == MouseAction.EButton.Back || _button == MouseAction.EButton.Forward)
                DllImport.UserInfo32.mouse_event(vk, 0, 0, x, 0);
            else if(_button != MouseAction.EButton.Undefined)
                DllImport.UserInfo32.mouse_event(vk, 0, 0, 0, 0);

            if(_scrollAxis == MouseAction.EAxis.X)
                DllImport.UserInfo32.mouse_event(DllImport.UserInfo32.MOUSEEVENTF_HWHEEL, 0, 0, (int)_speed, 0);
            else if(_scrollAxis == MouseAction.EAxis.Y)
                DllImport.UserInfo32.mouse_event(DllImport.UserInfo32.MOUSEEVENTF_WHEEL, 0, 0, (int)_speed, 0);
        }

        public void InvokeOff() {
            uint vk;
            int x = 0;
            switch (_button) {
                case MouseAction.EButton.Left:
                    vk = DllImport.UserInfo32.MOUSEEVENTF_LEFTUP;
                    break;
                case MouseAction.EButton.Middle:
                    vk = DllImport.UserInfo32.MOUSEEVENTF_MIDDLEUP;
                    break;
                case MouseAction.EButton.Right:
                    vk = DllImport.UserInfo32.MOUSEEVENTF_RIGHTUP;
                    break;
                case MouseAction.EButton.Forward:
                    vk = DllImport.UserInfo32.MOUSEEVENTF_XUP;
                    x = DllImport.UserInfo32.XBUTTON2;
                    break;
                case MouseAction.EButton.Back:
                    vk = DllImport.UserInfo32.MOUSEEVENTF_XUP;
                    x = DllImport.UserInfo32.XBUTTON1;
                    break;
                case MouseAction.EButton.Undefined:
                default:
                    return;
            }
            if (_button == MouseAction.EButton.Back || _button == MouseAction.EButton.Forward)
                DllImport.UserInfo32.mouse_event(vk, (int)_setPosition.X, (int)_setPosition.X, x, 0);
            else
                DllImport.UserInfo32.mouse_event(vk, (int)_setPosition.X, (int)_setPosition.X, 0, 0);
        }
    }
}
