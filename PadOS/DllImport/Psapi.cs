using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PadOS.DllImport
{
    public static class Psapi
    {
        public const int Maxtitle = 255;
        [DllImport("psapi.dll")]
        public static extern int GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In] [MarshalAs(UnmanagedType.U4)] int nSize);
    }
}