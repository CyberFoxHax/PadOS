using System;
using System.Windows;

namespace PadOS.Views {
	public partial class SystemTray{
		public SystemTray() {
			InitializeComponent();
		}

        public event Action ShowHideOnClick;

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

        private void ShowHide_OnClick(object sender, RoutedEventArgs e){
			new System.Threading.Thread(() =>{
				Dispatcher.BeginInvoke(new Action(() => {
                    ShowHideOnClick.Invoke();
				}));
			}).Start();
		}
	}
}
