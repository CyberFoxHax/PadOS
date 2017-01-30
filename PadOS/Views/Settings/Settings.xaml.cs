using System;
using System.Linq;

namespace PadOS.Views.Settings {
	public partial class Settings{
		public Settings() {
			InitializeComponent();

			SimpleGamePadNavagtion.Register(this, ButtonsList.Children.OfType<INavigatable>());
		}
	}
}
