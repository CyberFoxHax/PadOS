namespace PadOS {
	public partial class App {
		public App(){
			// todo create virtual controllers anor remap them

			var mainWindow = new Views.Main.MainWindow();
			mainWindow.Show();
			mainWindow.Closed += (sender, args) => System.Environment.Exit(0);
		}
	}
}
