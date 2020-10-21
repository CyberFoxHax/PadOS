using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using PadOS.Input.GamePadInput;
using XInputDotNetPure;
using Stopwatch = System.Diagnostics.Stopwatch;
using Timer = System.Timers.Timer;

namespace PadOS.Plugin.DesktopInput
{
    public class DesktopInputSimulator : PadOS.InputSimulatorPlugin {
        private GamePadInput _gamePadInput;
        private Input.Vector2 _cursorPosition;
        private Input.Vector2 _thumbStickValue;
        private float _leftTriggerValue;
        private float _rightTriggerValue;

        private float _leftTrigger_mouseSpeed = 3f;
        private float _rightTrigger_mouseSpeed = 0.15f;
        private float _leftTrigger_scrollSpeed = 10f;
        private float _rightTrigger_ScrollSpeed = 0.5f;
        private int _repeatButtonCode = 0;
        private int _dpadPadPressedButtonsCount = 0;

        private Timer _mouseMoveTimer = new Timer {
            Interval = 1, // not really 1ms. It locks on the lowest possible speed, which is the monitors refreshrate.
            AutoReset = true
        };

        private Timer _keyRepeatTimer = new Timer {
            Interval = 250,
            AutoReset = true,
            Enabled = false
        };

        public void Load() {
            _gamePadInput = new GamePadInput();
            _gamePadInput.IsEnabled = true;
            _gamePadInput.ThumbLeftChange += gamePadInput_ThumbLeftChange;
            _gamePadInput.TriggerLeftChange += (p, s, v) => _leftTriggerValue = v;
            _gamePadInput.TriggerRightChange += (p, s, v) => _rightTriggerValue = v;
            _gamePadInput.ButtonRightShoulderDown += (p, v) => keybd_event(VK_LSHIFT, 0x45, KEYEVENTF_KEYDOWN, 0);
            _gamePadInput.ButtonRightShoulderUp += (p, v) => keybd_event(VK_LSHIFT, 0x45, KEYEVENTF_KEYUP, 0);
            _gamePadInput.ButtonLeftShoulderDown += (p, v) => keybd_event(VK_LCONTROL, 0x45, KEYEVENTF_KEYDOWN, 0);
            _gamePadInput.ButtonLeftShoulderUp += (p, v) => keybd_event(VK_LCONTROL, 0x45, KEYEVENTF_KEYUP, 0);
            _gamePadInput.ButtonADown += (p, s) => mouse_event(MOUSEEVENTF_LEFTDOWN, (int)_cursorPosition.X, (int)_cursorPosition.Y, 0, 0);
            _gamePadInput.ButtonBDown += (p, s) => mouse_event(MOUSEEVENTF_MIDDLEDOWN, (int)_cursorPosition.X, (int)_cursorPosition.Y, 0, 0);
            _gamePadInput.ButtonXDown += (p, s) => mouse_event(MOUSEEVENTF_RIGHTDOWN, (int)_cursorPosition.X, (int)_cursorPosition.Y, 0, 0);
            _gamePadInput.ButtonYDown += (p, s) => keybd_event(VK_ESCAPE, 0x45, KEYEVENTF_KEYUP, 0);
            _gamePadInput.ButtonAUp += (p, s) => mouse_event(MOUSEEVENTF_LEFTUP, (int)_cursorPosition.X, (int)_cursorPosition.Y, 0, 0);
            _gamePadInput.ButtonBUp += (p, s) => mouse_event(MOUSEEVENTF_MIDDLEUP, (int)_cursorPosition.X, (int)_cursorPosition.Y, 0, 0);
            _gamePadInput.ButtonXUp += (p, s) => mouse_event(MOUSEEVENTF_RIGHTUP, (int)_cursorPosition.X, (int)_cursorPosition.Y, 0, 0);
            _gamePadInput.DPadDownDown += GetDPadHandlerDown(VK_DOWN);
            _gamePadInput.DPadDownUp += GetDPadHandlerUp(VK_DOWN);
            _gamePadInput.DPadUpDown += GetDPadHandlerDown(VK_UP);
            _gamePadInput.DPadUpUp += GetDPadHandlerUp(VK_UP);
            _gamePadInput.DPadLeftDown += GetDPadHandlerDown(VK_LEFT);
            _gamePadInput.DPadLeftUp += GetDPadHandlerUp(VK_LEFT);
            _gamePadInput.DPadRightDown += GetDPadHandlerDown(VK_RIGHT);
            _gamePadInput.DPadRightUp += GetDPadHandlerUp(VK_RIGHT);
            _gamePadInput.ThumbRightChange += gamePadInput_ThumbRightChange;
            _keyRepeatTimer.Elapsed += keyRepeatTimer_Elapsed;
            _mouseMoveTimer.Elapsed += MouseMoveTimer_Tick;
            _mouseScrollTimer.Elapsed += MouseScrollTimer_Tick;
            SetupHoldHotkeys();
        }

        private void keyRepeatTimer_Elapsed(object sender, ElapsedEventArgs e) {
            _keyRepeatTimer.Interval = 1000d / 40;
            keybd_event(_repeatButtonCode, 0, KEYEVENTF_KEYDOWN, 0);
            keybd_event(_repeatButtonCode, 0, KEYEVENTF_KEYUP, 0);
        }

        private PadOS.Input.GamePadEvent GetDPadHandlerUp(int vkey) {
            return (p, s) => {
                _dpadPadPressedButtonsCount--;
                if(_dpadPadPressedButtonsCount == 0)
                    _keyRepeatTimer.Stop();
                keybd_event(vkey, 0, KEYEVENTF_KEYUP, 0);
            };
        }
        private PadOS.Input.GamePadEvent GetDPadHandlerDown(int vkey) {
            return (p, s) => {
                _dpadPadPressedButtonsCount++;
                _repeatButtonCode = vkey;
                keybd_event(vkey, 0, KEYEVENTF_KEYDOWN, 0);
                _keyRepeatTimer.Interval = 500;
                _keyRepeatTimer.Start();
            };
        }

        private Stopwatch _rightStickStopwatch = new Stopwatch();
        private Stopwatch _leftStickStopwatch = new Stopwatch();
        private Timer _rightStickTickTimer = new Timer { AutoReset = true, Interval = 500 };
        private Timer _leftStickTickTimer = new Timer { AutoReset = true, Interval = 500 };

        private void SetupHoldHotkeys() {
            _gamePadInput.ButtonLeftStickDown += gamePadInput_ButtonLeftStickDown;
            _gamePadInput.ButtonLeftStickUp += gamePadInput_ButtonLeftStickUp;
            _gamePadInput.ButtonRightStickDown += gamePadInput_ButtonRightStickDown;
            _gamePadInput.ButtonRightStickUp += gamePadInput_ButtonRightStickUp;


            /* bug, because some some black magic sideeffect,
             * commenting out the altf4 instruction will cancel vibration*/
            _rightStickTickTimer.Elapsed += async (s, e) => {
                await Vibrate(); 
                if (Math.Floor(_rightStickStopwatch.ElapsedMilliseconds/100d)*100 >= 500) {
                    _rightStickTickTimer.Stop();
                    Hotkey_AltF4();
                    return;
                }
            };
            _leftStickTickTimer.Elapsed += (s, e) => {
                Vibrate();
                if (Math.Floor(_leftStickStopwatch.ElapsedMilliseconds/100d)*100 >= 1000) {
                    _leftStickTickTimer.Stop();
                    Hotkey_CtrlX();
                    return;
                }
            };
        }

        private async Task Vibrate() {
            _gamePadInput.SetVibration(0, 1, 1);
            await Task.Delay(100);
            _gamePadInput.SetVibration(0, 0, 0);
        }

        private async void gamePadInput_ButtonRightStickDown(int player, GamePadState state) {
            _rightStickStopwatch.Reset();
            _rightStickStopwatch.Start();
            _rightStickTickTimer.Start();
        }

        private async void gamePadInput_ButtonLeftStickDown(int player, GamePadState state) {
            _leftStickStopwatch.Reset();
            _leftStickStopwatch.Start();
            _leftStickTickTimer.Start();
        }

        private void gamePadInput_ButtonRightStickUp(int player, GamePadState state) {
            _rightStickTickTimer.Stop();
            if (_rightStickStopwatch.ElapsedMilliseconds < 500) {
                Hotkey_AltTab();
            }
        }

        private void gamePadInput_ButtonLeftStickUp(int player, GamePadState state) {
            _leftStickTickTimer.Stop();
            if (_leftStickStopwatch.ElapsedMilliseconds < 500) {
                Hotkey_CtrlV();
            }
            else if (_leftStickStopwatch.ElapsedMilliseconds < 1000) {
                Hotkey_CtrlC();
            }
        }

        private static void Hotkey_CtrlC() => Hotkey(VK_LCONTROL, VK_C);
        private static void Hotkey_CtrlX() => Hotkey(VK_LCONTROL, VK_X);
        private static void Hotkey_CtrlV() => Hotkey(VK_LCONTROL, VK_V);
        private static void Hotkey_AltTab() => Hotkey(VK_MENU, VK_TAB);
        private static void Hotkey_AltF4() => Hotkey(VK_MENU, VK_F4);

        private static void Hotkey(int btn1, int btn2) {
            keybd_event(btn1, 0, KEYEVENTF_KEYDOWN, 0);
            keybd_event(btn2, 0, KEYEVENTF_KEYDOWN, 0);
            keybd_event(btn2, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(btn1, 0, KEYEVENTF_KEYUP, 0);
        }

        private void MouseMoveTimer_Tick(object sender, EventArgs e) {
            var val = _thumbStickValue;
            val.Y = -val.Y;
            // todo, these two equations may actually be one?
            val *= 1 + (_leftTrigger_mouseSpeed - 1) * _leftTriggerValue;
            val *= 1 - (1 - _rightTrigger_mouseSpeed) * _rightTriggerValue;
            _cursorPosition += val * 20f;

            var screenSize = Screen.PrimaryScreen.Bounds;
            if (_cursorPosition.X < 0)
                _cursorPosition.X = 0;
            else if (_cursorPosition.X > screenSize.Width)
                _cursorPosition.X = screenSize.Width;

            if (_cursorPosition.Y < 0)
                _cursorPosition.Y = 0;
            else if (_cursorPosition.Y > screenSize.Height)
                _cursorPosition.Y = screenSize.Height;

            Cursor.Position = new System.Drawing.Point((int)_cursorPosition.X, (int)_cursorPosition.Y);
        }

        private void gamePadInput_ThumbLeftChange(int player, GamePadState state, Input.Vector2 value) {
            _thumbStickValue = value;
            if (_thumbStickValue.GetSquared() == 0d && _mouseMoveTimer.Enabled == true) {
                _mouseMoveTimer.Enabled = false;
            }
            if (_thumbStickValue.GetSquared() != 0d && _mouseMoveTimer.Enabled == false) {
                _cursorPosition = new Input.Vector2(Cursor.Position.X, Cursor.Position.Y);
                _mouseMoveTimer.Enabled = true;
            }
        }

        public void Unload() {
            _gamePadInput.IsEnabled = false;
            _gamePadInput.Dispose();
            _mouseMoveTimer.Stop();
            _mouseMoveTimer.Elapsed -= MouseMoveTimer_Tick;
            _mouseMoveTimer.Dispose();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void keybd_event(int bVk, int bScan, uint dwFlags, uint dwExtraInfo);
        private const int KEYEVENTF_KEYDOWN = 0x0000;
        private const int KEYEVENTF_KEYUP = 0x0002;
        private const int VK_LCONTROL = 0x00A2;
        private const int VK_LSHIFT = 0x00A0;
        private const int VK_MENU = 0x0012;
        private const int VK_ESCAPE = 0x001B;
        private const int VK_F4 = 0x0073;
        private const int VK_TAB = 0x0009;
        private const int VK_LEFT = 0x0025;
        private const int VK_UP = 0x0026;
        private const int VK_RIGHT = 0x0027;
        private const int VK_DOWN = 0x0028;
        private const int VK_C = 0x0043;
        private const int VK_X = 0x0058;
        private const int VK_V = 0x0056;


        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, uint dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const int MOUSEEVENTF_WHEEL = 0x0800;
        private const int MOUSEEVENTF_HWHEEL = 0x1000;

        private System.Timers.Timer _mouseScrollTimer = new System.Timers.Timer {
            Interval = 1,
            AutoReset = true
        };
        private Input.Vector2 _rightStickValue;

        private void gamePadInput_ThumbRightChange(int player, GamePadState state, Input.Vector2 value) {
            _rightStickValue = value;

            if ((value.Y != 0 || value.X != 0) && _mouseScrollTimer.Enabled == false) {
                _mouseScrollTimer.Enabled = true;
            }
            else if ((value.Y == 0 && value.X == 0) && _mouseScrollTimer.Enabled == true) {
                _mouseScrollTimer.Enabled = false;
            }
        }

        private void MouseScrollTimer_Tick(object sender, ElapsedEventArgs e) {
            // todo horizontal scroll has no length parameter
            var val = _rightStickValue * 50;
            // todo, these two equations may actually be one?
            val *= 1 + (_leftTrigger_scrollSpeed - 1) * _leftTriggerValue;
            val *= 1 - (1 - _rightTrigger_ScrollSpeed) * _rightTriggerValue;
            if (val.Y != 0)
                mouse_event(MOUSEEVENTF_WHEEL , (int)_cursorPosition.X, (int)_cursorPosition.Y, (int)val.Y, 0);

            if (val.X != 0)
                mouse_event(MOUSEEVENTF_HWHEEL, (int)_cursorPosition.X, (int)_cursorPosition.X, (int)val.X, 0);
        }
    }
}
