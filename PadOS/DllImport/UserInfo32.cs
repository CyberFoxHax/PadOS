using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PadOS.DllImport
{
    public static class UserInfo32
    {
        public const int WmSyscommand = 0x112;
        public const int ScMinimize = 0xF020;
        public const int ScClose = 0xF060;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowModuleFileName(IntPtr hwnd, StringBuilder lpszFileName, int cchFileNameMax);

        public delegate bool EnumDelegate(IntPtr hWnd, int lParam);

        [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "GetWindowText", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool CloseWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void keybd_event(int bVk, int bScan, uint dwFlags, uint dwExtraInfo);
        public const int KEYEVENTF_KEYDOWN = 0x00;
        public const int KEYEVENTF_EXTENDEDKEY = 0x01;
        public const int KEYEVENTF_KEYUP = 0x02;
        public const int VK_LCONTROL = 0xA2;
        public const int VK_LSHIFT = 0xA0;
        public const int VK_MENU = 0x12; // ALT
        public const int VK_ESCAPE = 0x1B;
        public const int VK_F4 = 0x73;
        public const int VK_TAB = 0x09;
        public const int VK_LEFT = 0x25;
        public const int VK_UP = 0x26;
        public const int VK_RIGHT = 0x27;
        public const int VK_DOWN = 0x28;
        public const int VK_BACK = 0x08;
        public const int VK_DELETE = 0x2E;
        public const int VK_RETURN = 0x0D;
        public const int VK_SPACE = 0x20;
        public const int VK_LWIN = 0x5B;

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, uint dwExtraInfo);
        public const int MOUSEEVENTF_MOVE = 0x01;
        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        public const int MOUSEEVENTF_MIDDLEUP = 0x40;
        public const int MOUSEEVENTF_WHEEL = 0x800;
        public const int MOUSEEVENTF_HWHEEL = 0x1000;
        public const int MOUSEEVENTF_XDOWN = 0x80;
        public const int MOUSEEVENTF_XUP = 0x100;
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        public const int XBUTTON1 = 0x1;
        public const int XBUTTON2 = 0x2;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCursorPos(POINT p);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out POINT point);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT {
            public POINT(int _x, int _y) {
                x = _x;
                y = _y;
            }
            public int x;
            public int y;
            public override string ToString() {
                return $"{nameof(POINT)}({x}, {y})";
            }
        }

        public static POINT GetCursorPos() {
            if (GetCursorPos(out POINT point))
                return point;
            return new POINT();
        }
    }
}
