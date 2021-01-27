﻿using System.Linq;
using System.Windows;

namespace PadOS.Views.Settings
{
    public class ActiveApplication {

        public static string ApplicationName {
            get {
                var hWnd = DllImport.UserInfo32.GetForegroundWindow();
                int processId;
                DllImport.UserInfo32.GetWindowThreadProcessId(hWnd, out processId);
                var res = System.Diagnostics.Process.GetProcesses().FirstOrDefault(p => p.Id == processId);
                return res?.ProcessName;
            }
        }

    }
}
