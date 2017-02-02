using System;
using System.Windows;

namespace PadOS.Views {
	public partial class SystemTray{
		public SystemTray() {
			InitializeComponent();
		}

		private void Exit_OnClick(object sender, RoutedEventArgs e){
			new System.Threading.Thread(() =>{
				// allow for the context menu to fade out
				System.Threading.Thread.Sleep(200); 
				Dispatcher.BeginInvoke(new Action(() => {
					Dispose();
					Application.Current.Shutdown();
					Environment.Exit(0);
				}));
			}).Start();
		}
	}
}
