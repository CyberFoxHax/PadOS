using System;
using System.Linq;

namespace PadOS.Views.Settings {
	public partial class Settings : Input.IGamePadFocusable{
		public Settings() {
			InitializeComponent();

			Input.WPFGamepad.Focus(this);
			VerticalGamePadNavagtion.Register(this, ButtonsList.Children.OfType<INavigatable>(), Dispatcher);
		}

		public bool IsGamePadFocused { get; set; }
	}
}
