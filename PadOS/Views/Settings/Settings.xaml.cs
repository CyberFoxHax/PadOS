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

		public bool IsGamePadFocused { get; set; }
	}
}
