using System.Runtime.InteropServices;

namespace PadOS.Plugin.DesktopInput
{
    public static class ScrollWheelSpeedModifier {
        private static uint _oldScrollLines = 3;
        private static uint _oldScrollChars = 3;

        // 1 second delay
        public static void Set(uint value=1) {
            SystemParametersInfo(SPI_GETWHEELSCROLLLINES, 0, ref _oldScrollLines, 0);
            SystemParametersInfo(SPI_GETWHEELSCROLLCHARS, 0, ref _oldScrollChars, 0);
            SystemParametersInfoW(SPI_SETWHEELSCROLLLINES, value, 0, 3);
            SystemParametersInfoW(SPI_SETWHEELSCROLLCHARS, value, 0, 3);
        }

        // 1 second delay
        public static void Reset() {
            SystemParametersInfoW(SPI_SETWHEELSCROLLLINES, _oldScrollLines, 0, 3);
            SystemParametersInfoW(SPI_SETWHEELSCROLLCHARS, _oldScrollChars, 0, 3);
        }

        [DllImport("User32.dll")]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref uint oVar, uint fWinIni);
        private const int SPI_GETWHEELSCROLLLINES = 0x0068;
        private const int SPI_GETWHEELSCROLLCHARS = 0x006C;
        [DllImport("User32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern bool SystemParametersInfoW(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);
        private const int SPI_SETWHEELSCROLLLINES = 0x0069;
        private const int SPI_SETWHEELSCROLLCHARS = 0x006D;
    }
}
