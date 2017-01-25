
namespace PadOS {
	public partial class App {
		public App(){
			// todo create virtual controllers anor remap them

			SaveData.MainPanel.Load();

			var mainWindow = new Views.MainPanel.MainPanel();
			mainWindow.Show();
			mainWindow.Closed += (sender, args) => System.Environment.Exit(0);
		}
	}
}
