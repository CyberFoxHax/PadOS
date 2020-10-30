using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PadOS.Commands
{
    public static class WindowCommands {
		static WindowCommands() {
			WindowMinimizeCommand = new RelayCommand(WindowMinimize);
			WindowCloseCommand = new RelayCommand(WindowClose);
			WindowKillCommand = new RelayCommand(WindowKill);
		}

		public static ICommand WindowKillCommand { get; private set; }
		public static ICommand WindowCloseCommand { get; private set; }
		public static ICommand WindowMinimizeCommand { get; private set; }

		private static void WindowMinimize() {
			var lHwnd = Plugins.UserInfo32.GetForegroundWindow();
			Plugins.UserInfo32.SendMessage(lHwnd, Plugins.UserInfo32.WmSyscommand, Plugins.UserInfo32.ScMinimize, 0);
		}

		private static void WindowClose() {
			var lHwnd = Plugins.UserInfo32.GetForegroundWindow();
			Plugins.UserInfo32.SendMessage(lHwnd, Plugins.UserInfo32.WmSyscommand, Plugins.UserInfo32.ScClose, 0);
		}

		private static void WindowKill() {
			var hWnd = Plugins.UserInfo32.GetForegroundWindow();

			int processId;
			Plugins.UserInfo32.GetWindowThreadProcessId(hWnd, out processId);

			var current = System.Diagnostics.Process.GetCurrentProcess();
			if (current.Id == processId) return;

			var firstOrDefault = System.Diagnostics.Process.GetProcesses().FirstOrDefault(p => p.Id == processId);
			firstOrDefault?.Kill();
		}
	}
}