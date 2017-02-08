using System;
using System.Linq;

namespace PadOS.Views.Settings {
	public partial class Settings : Input.IGamePadFocusable{
		public Settings() {
			InitializeComponent();

			Input.WPFGamepad.Focus(this);
			VerticalGamePadNavagtion.Register(this, ButtonsList.Children.OfType<INavigatable>(), Dispatcher);

			GlobalEvents.HomeClose += GlobalEventsOnHomeClose;
		}

		private void GlobalEventsOnHomeClose(bool isopen){
			GlobalEvents.HomeClose -= GlobalEventsOnHomeClose;
			Dispatcher.BeginInvoke(new Action(Close));
		}

		private void Window_OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
				DragMove();
		}

		public bool IsGamePadFocused { get; set; }
	}
}
