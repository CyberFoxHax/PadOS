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
			frameworkElement.Dispatcher.BeginInvoke(new System.Action(() => {
				System.Windows.Forms.Application.Restart();
			}));
		}

		public static void Shutdown(System.Windows.FrameworkElement frameworkElement) {
			frameworkElement.Dispatcher.BeginInvoke(new System.Action(() => {
				System.Windows.Application.Current.Shutdown();
				System.Environment.Exit(0);
			}));
		}
	}
}