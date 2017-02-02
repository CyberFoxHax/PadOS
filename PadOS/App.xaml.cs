
namespace PadOS {
	public partial class App {
		public App(){
#if DEBUG
#if x64
			const bool is64BitBuild = true;
#else
			const bool is64BitBuild = false;
#endif

			if (System.Environment.Is64BitOperatingSystem != is64BitBuild)
				throw new System.Exception("You need to run in in x64 mode");
#endif

			SaveData.SaveDataUtils.Init();
		}

		protected override void OnStartup(System.Windows.StartupEventArgs e){
			base.OnStartup(e);
			var systray = new Views.SystemTray();
			var mainWindow = new Views.MainPanel.MainPanel();

			if (System.Diagnostics.Debugger.IsAttached == false) return;
			GlobalEvents.InterfaceIsOpen = true;
			GlobalEvents.Init();
			mainWindow.Show();
		}
	}
}
