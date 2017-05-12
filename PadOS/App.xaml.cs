
using System.Windows.Threading;
using PadOS.Navigation;

namespace PadOS {
	public partial class App{
		public static Dispatcher GlobalDispatcher { get; private set; }

		public App(){
			GlobalDispatcher = Dispatcher;
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

			//GlobalEvents.Initialize();
			Navigator.Initialize();
			if (System.Diagnostics.Debugger.IsAttached == false) return;
			Navigator.OpenMainPanel();
		}
	}
}
