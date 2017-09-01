using System.Windows.Input;

namespace PadOS.Commands{
	public static class ApplicationCommands{
		static ApplicationCommands(){
			RestartCommand	= new RelayCommand<System.Windows.FrameworkElement>(Restart);
			ShutdownCommand = new RelayCommand<System.Windows.FrameworkElement>(Shutdown);
		}

		public static ICommand RestartCommand { get; private set; }
		public static ICommand ShutdownCommand { get; private set; }

		public static void Restart(System.Windows.FrameworkElement frameworkElement){
				System.Windows.Forms.Application.Restart();
				System.Windows.Application.Current.Shutdown();
		}

		public static void Shutdown(System.Windows.FrameworkElement frameworkElement) {
				System.Windows.Application.Current.Shutdown();
		}
	}
}