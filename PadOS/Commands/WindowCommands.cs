using System.Linq;
using System.Windows.Input;

namespace PadOS.Commands {
	public static class WindowCommands {
		public static string GetApplicationName {
			get {
				// todo bug: wrong result
				// because the value is gotten once onload
				var hWnd = Dll.UserInfo32.GetForegroundWindow();
				int processId;
				Dll.UserInfo32.GetWindowThreadProcessId(hWnd, out processId);
				var res = System.Diagnostics.Process.GetProcesses().FirstOrDefault(p => p.Id == processId);
				return res?.ProcessName;
			}
		}

		static WindowCommands() {
			WindowMinimizeCommand = new RelayCommand(WindowMinimize);
			WindowCloseCommand = new RelayCommand(WindowClose);
			WindowKillCommand = new RelayCommand(WindowKill);
		}

		public static ICommand WindowKillCommand { get; private set; }
		public static ICommand WindowCloseCommand { get; private set; }
		public static ICommand WindowMinimizeCommand { get; private set; }

		private static void WindowMinimize() {
			var lHwnd = Dll.UserInfo32.GetForegroundWindow();
			Dll.UserInfo32.SendMessage(lHwnd, Dll.UserInfo32.WmSyscommand, Dll.UserInfo32.ScMinimize, 0);
		}

		private static void WindowClose() {
			var lHwnd = Dll.UserInfo32.GetForegroundWindow();
			Dll.UserInfo32.SendMessage(lHwnd, Dll.UserInfo32.WmSyscommand, Dll.UserInfo32.ScClose, 0);
		}

		private static void WindowKill() {
			var hWnd = Dll.UserInfo32.GetForegroundWindow();

			int processId;
			Dll.UserInfo32.GetWindowThreadProcessId(hWnd, out processId);

			var current = System.Diagnostics.Process.GetCurrentProcess();
			if (current.Id == processId) return;

			var firstOrDefault = System.Diagnostics.Process.GetProcesses().FirstOrDefault(p => p.Id == processId);
			if (firstOrDefault != null)
				firstOrDefault.Kill();
		}
	}
}