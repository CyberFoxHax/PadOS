using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
