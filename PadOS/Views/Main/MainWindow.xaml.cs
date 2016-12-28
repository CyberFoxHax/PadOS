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
	public partial class MainWindow{
		public MainWindow() {
			InitializeComponent();

			var gamepadInput = Input.WPFGamepad.Register(this);
			gamepadInput.GamePadOnChange += GamepadOnChange;
		}

		private void GamepadOnChange(object sender, XInputDotNetPure.GamePadState input){
			// todo manager player number

			var angle = Math.Atan2(input.ThumbSticks.Left.X, input.ThumbSticks.Left.Y)*180/Math.PI;

			AngleText.Text = angle.ToString("F0") + "°";
			ScaleTransform.ScaleX = ScaleTransform.ScaleY = input.Triggers.Left + 1-input.Triggers.Right;
			RotateTransform.Angle = angle;
		}
	}
}
