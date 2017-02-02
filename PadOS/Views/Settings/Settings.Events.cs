using System;
using System.Linq;
using System.Text;

namespace PadOS.Views.Settings {
	public partial class Settings {
		private void ChangePlayers_OnActivate() {
		}

		private void EditGamePads_OnActivate() {
		}

		private void EditMainPanel_OnActivate() {
		}

		private void EditKeyboardMappings_OnActivated() {
		}

		private void EditKeyboardProfiles_OnActivated() {
		}

		private void EditGamePadCalibration_OnActivate() {
		}

		private void EditMainPanelProfile_OnActivate() {
		}

		private void WindowMinimize_OnActivate() {
			var lHwnd = UserInfo32.GetForegroundWindow();
			UserInfo32.SendMessage(lHwnd, UserInfo32.WmSyscommand, UserInfo32.ScMinimize, 0);
		}

		private void WindowClose_OnActivate(){
			var window = UserInfo32.GetForegroundWindow();
			UserInfo32.SendMessage(window, UserInfo32.WmSyscommand, UserInfo32.ScClose, 0);
		}

		private void WindowKill_OnActivate(){
			var hWnd = UserInfo32.GetForegroundWindow();

			var strBuild = new StringBuilder{Length = 0};

			int processId;
			UserInfo32.GetWindowThreadProcessId(hWnd, out processId);

			var processPtr = UserInfo32.OpenProcess(UserInfo32.ProcessAccessFlags.QueryInformation | UserInfo32.ProcessAccessFlags.VirtualMemoryRead, true, processId);

			UserInfo32.GetModuleFileNameEx(processPtr, IntPtr.Zero, strBuild, UserInfo32.Maxtitle);

			var ext = strBuild.ToString();

			var firstOrDefault = System.Diagnostics.Process.GetProcesses().FirstOrDefault(p => p.MainModule.FileName == ext);
			if (firstOrDefault != null)
				firstOrDefault.Kill();
		}

		private void RestartPadOS_OnActivate(){
		}


		private void ShutdownPadOS_OnActivate() {
			Dispatcher.BeginInvoke(new Action(() => {
				Close();
				System.Windows.Application.Current.Shutdown();
				Environment.Exit(0);
			}));
		}
	}
}
