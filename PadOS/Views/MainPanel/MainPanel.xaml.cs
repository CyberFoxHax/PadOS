using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PadOS.Views.MainPanel {
	public partial class MainPanel{
		public MainPanel() {
			InitializeComponent();
			Highlight.Visibility = Visibility.Hidden;

			var gamepadInput = Input.WPFGamepad.Register(this);
			Input.WPFGamepad.XInput.ButtonGuideDown += XInputOnButtonGuideDown;
			gamepadInput.ThumbLeftChange += GamepadInputOnThumbLeftChange;
			gamepadInput.ButtonADown += GamepadInputOnButtonADown;

			var elms = Canvas.Children.OfType<FrameworkElement>().ToArray();
			const int upper = 8;
			const double tau = Math.PI * 2;
			const double segment = tau/upper;
			for (var i = 0; i < upper; i++){
				Canvas.SetLeft(elms[i], Math.Cos(segment * i - segment * 2) * 270 + Width / 2);
				Canvas.SetTop (elms[i], Math.Sin(segment * i - segment * 2) * 270 + Width / 2);
			}
		}

		private Vector2 _leftStick;

		private void GamepadInputOnButtonADown(object sender, Input.WPFGamepad.GamePadEventArgs args){
			Highlight.Visibility = Visibility.Hidden;
		}


		private void GamepadInputOnThumbLeftChange(object sender, Input.WPFGamepad.GamePadEventArgs<Vector2> args){
			_leftStick = args.Value;
			if(args.Value.GetLength() > 0.1){
				var angle = args.Value.GetAngle();
				HighlightRotate.Angle = Math.Round((angle / Math.PI * 180 + 90) / 45) * 45;
				Highlight.Visibility = Visibility.Visible;
			}
			else{
				Highlight.Visibility = Visibility.Hidden;
			}
		}

		private void XInputOnButtonGuideDown(XInputDotNetPure.PlayerIndex player, XInputDotNetPure.GamePadState state) {
			Dispatcher.BeginInvoke(new Action(() => {
				if (IsVisible)
					Hide();
				else
					Show();
			}));
		}
	}
}
