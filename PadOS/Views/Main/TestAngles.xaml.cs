﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PadOS.Views.Main {
	public class AngleItem : Canvas {
		public AngleItem(){
			Children.Add(_ellipse);
			Children.Add(_textBlock);
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo){
			_ellipse.Height = Height;
			_ellipse.Width = Height;
			SetTop(_textBlock, _ellipse.Height);
			base.OnRenderSizeChanged(sizeInfo);
		}

		private readonly Ellipse _ellipse = new Ellipse();
		private readonly TextBlock _textBlock = new TextBlock();

		public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
			"IsActive", typeof (bool), typeof (AngleItem), new PropertyMetadata(default(bool)));

		public bool IsActive
		{
			get { return (bool) GetValue(IsActiveProperty); }
			set { SetValue(IsActiveProperty, value); }
		}

		public Brush Fill
		{
			get { return _ellipse.Fill; }
			set { _ellipse.Fill = value; }
		}

		public string Text {
			get { return _textBlock.Text; }
			set { _textBlock.Text = value; }
		}
	}
	public partial class TestAngles{
		

		public TestAngles() {
			InitializeComponent();

			var gamepadInput = Input.WPFGamepad.Register(this);
			gamepadInput.GamePadOnChange += GamepadOnChange;

			_activeEllipse = Canvas.Children.OfType<AngleItem>().First(p => p.IsActive);
		}

		private AngleItem _activeEllipse;
		private bool _waitNav;

		private void GamepadOnChange(object sender, XInputDotNetPure.GamePadState input){
			var jsAngle = Math.Atan2(input.ThumbSticks.Left.X, input.ThumbSticks.Left.Y) * 180 / Math.PI;
			RotateTransform.Angle = jsAngle;
			AngleText.Text = jsAngle.ToString("F0") + "°";

			var children = Canvas.Children.OfType<AngleItem>().ToArray();

			foreach (var angleItem in children){
				angleItem.IsActive = false;
				angleItem.Fill = Brushes.DarkGray;

				var distance = Math.Sqrt(
					Math.Pow(Canvas.GetLeft(angleItem) - Canvas.GetLeft(_activeEllipse), 2) +
					Math.Pow(Canvas.GetTop (angleItem) - Canvas.GetTop (_activeEllipse), 2)
				);

				var diffX = Canvas.GetLeft(angleItem) - Canvas.GetLeft(_activeEllipse);
				var diffY = Canvas.GetTop (_activeEllipse) - Canvas.GetTop (angleItem);

				var angle = Math.Atan2(diffX, diffY) * 180 / Math.PI;

				var angleDiff = Math.Abs(jsAngle - angle);

				angleItem.Text = string.Format("A:{0:f0}", angleDiff);

				if (angleDiff < 45)
					angleItem.Fill = Brushes.CornflowerBlue;
			}

			if(_activeEllipse != null){
				Canvas.SetLeft(PointerImage, Canvas.GetLeft(_activeEllipse) - PointerImage.Width  / 2 + _activeEllipse.Width  / 2);
				Canvas.SetTop (PointerImage, Canvas.GetTop (_activeEllipse) - PointerImage.Height / 2 + _activeEllipse.Height / 2);
				_activeEllipse.Fill = Brushes.GreenYellow;
			}

			if (input.Buttons.A == XInputDotNetPure.ButtonState.Pressed && _waitNav == false){
				_waitNav = true;
			}
			else if (input.Buttons.A == XInputDotNetPure.ButtonState.Released && _waitNav){
				_waitNav = false;
				Func<FrameworkElement, Vector2> getPos = p => new Vector2(
					Canvas.GetLeft(p),
					Canvas.GetTop (p)
				);

				var activePos = getPos(_activeEllipse);

				var allElements = (
					from elm in children
					let elmPos = getPos(elm)
					let diff = new Vector2(
						elmPos.X - activePos.X,
						activePos.Y - elmPos.Y
					)
					let angle = Math.Atan2(diff.X, diff.Y)*180/Math.PI
					let angleDiff = Math.Abs(jsAngle - angle)

					let diffDist = (diff*diff)
					let distance = Math.Sqrt(diffDist.X + diffDist.Y) 

					select new {
						Element = elm,
						AngleDiff = angleDiff,
						Distance = distance
					}
				).ToArray();

				var res = (
					from elm in allElements
					where elm.AngleDiff < 45
					orderby elm.AngleDiff
					orderby elm.Distance
					select elm.Element
				).FirstOrDefault();

				if (res != null)
					_activeEllipse = res;
			}
		}
		 
	}
}
