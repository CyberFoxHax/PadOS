
namespace PadOS {
	public partial class App {
		public App(){
			// todo create virtual controllers anor remap them

#if DEBUG
#if x64
			const bool is64BitBuild = true;
#else
			const bool is64BitBuild = false;
#endif

			var asm = System.Reflection.Assembly.GetExecutingAssembly();
			if (System.Environment.Is64BitOperatingSystem != is64BitBuild)
				throw new System.Exception("You need to run in in x64 mode");
#endif

			SaveData.MainPanel.Load();

			var mainWindow = new Views.MainPanel.MainPanel();
			mainWindow.Show();
			mainWindow.Closed += (sender, args) => System.Environment.Exit(0);
		}
	}
}
