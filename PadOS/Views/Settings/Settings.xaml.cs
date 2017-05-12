using System;
using System.Linq;

namespace PadOS.Views.Settings {
	public partial class Settings : Input.IGamePadFocusable{
		public Settings() {
			InitializeComponent();
			VerticalGamePadNavagtion.Register(this, ButtonsList.Children.OfType<INavigatable>(), Dispatcher);
		}

		private void Window_OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
				DragMove();
		}

		public bool IsGamePadFocused { get; set; }
	}
}
