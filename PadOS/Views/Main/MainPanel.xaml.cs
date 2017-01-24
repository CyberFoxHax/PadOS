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
using System.Windows.Shapes;

namespace PadOS.Views.Main {
	public partial class MainPanel{
		public MainPanel() {
			InitializeComponent();
			Highlight.Visibility = Visibility.Hidden;

			var gamepadInput = Input.WPFGamepad.Register(this);
			Input.WPFGamepad.XInput.ButtonGuideDown += XInputOnButtonGuideDown;
			gamepadInput.ThumbLeftChange += GamepadInputOnThumbLeftChange;
			gamepadInput.ButtonADown += GamepadInputOnButtonADown;
			
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
