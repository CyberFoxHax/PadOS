using System;
using System.Linq;

namespace PadOS.Views.Settings {
	public partial class Settings{
		public Settings() {
			InitializeComponent();

			var evts = Input.WPFGamepad.Register(this);
			evts.DPadDownDown += OnDPadDownDown;
			evts.DPadUpDown += OnDPadUpDown;
			evts.DPadDownUp += OnDPadDownUp;
			evts.DPadUpUp += OnDPadUpUp;

			var buttons = ButtonsList.Children.OfType<NavigationListItem>().ToArray();
			foreach (var button in buttons){
				button.IsActive = false;
			}
			_activeItem = buttons[0];
			_activeItem.IsActive = true;
		}

		private readonly System.Timers.Timer _holdDelay = new System.Timers.Timer(200){AutoReset = false};

		private void OnDPadUpDown(object sender, Input.WPFGamepad.GamePadEventArgs args){
			MoveUp();
		}

		private void OnDPadDownDown(object sender, Input.WPFGamepad.GamePadEventArgs args){
			MoveDown();
		}

		private void OnDPadUpUp(object sender, Input.WPFGamepad.GamePadEventArgs args) {
			_holdDelay.Stop();
		}

		private void OnDPadDownUp(object sender, Input.WPFGamepad.GamePadEventArgs args) {
			_holdDelay.Stop();
		}

		private void MoveUp(){
			var buttons = ButtonsList.Children.OfType<NavigationListItem>().ToArray();
			var index = Array.IndexOf(buttons, _activeItem);
			_activeItem.IsActive = false;
			_activeItem = buttons[index <= 0 ? buttons.Length - 1 : index - 1];
			_activeItem.IsActive = true;
		}

		private void MoveDown(){
			var buttons = ButtonsList.Children.OfType<NavigationListItem>().ToArray();
			var index = Array.IndexOf(buttons, _activeItem);
			_activeItem.IsActive = false;
			_activeItem = buttons[(index + 1) % buttons.Length];
			_activeItem.IsActive = true;
		}

		private NavigationListItem _activeItem;

	}
}
