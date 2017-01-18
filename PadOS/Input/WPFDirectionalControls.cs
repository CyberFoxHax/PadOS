using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using XInputDotNetPure;

namespace PadOS.Input {
	public class WPFDirectionalControls{

		public static WPFDirectionalControls Register(UIElement control) {
			var dirControl = new WPFDirectionalControls();
			var container = WPFGamepad.Register(control);
			container.ThumbLeftChange += dirControl.OnThumbChange;
			container.DPadDownDown	+= (sender, args) => dirControl.OnDPad(sender, new Vector2(0,  1));
			container.DPadUpDown	+= (sender, args) => dirControl.OnDPad(sender, new Vector2(0, -1));
			container.DPadLeftDown	+= (sender, args) => dirControl.OnDPad(sender, new Vector2(-1, 0));
			container.DPadRightDown	+= (sender, args) => dirControl.OnDPad(sender, new Vector2( 1, 0));
			return dirControl;
		}

		private const double ResetThreshold = 0.5;
		private bool _waitForReset;

		private void OnDPad(object sender, Vector2 vector2){
			var elm = sender as FrameworkElement;
			if (elm == null) return;

			var res = GetSelection(elm, vector2);
			res.Focus();
		}

		private void OnThumbChange(object sender, WPFGamepad.GamePadEventArgs<Vector2> args){
			var elm = sender as FrameworkElement;
			if (elm == null) return;

			var gamePadState = args.GamePadState;
			var vector = new Vector2(gamePadState.ThumbSticks.Left.X, gamePadState.ThumbSticks.Left.Y);
			var thumbLength = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (_waitForReset && thumbLength > ResetThreshold) return;

			if (thumbLength < ResetThreshold)
				_waitForReset = false;
			else if(_waitForReset)
				return;

			if (thumbLength < ResetThreshold) return;

			var res = GetSelection(elm, new Vector2(
				vector.X,
				vector.Y
				));
			if (res == null) return;
			_waitForReset = true;
			res.Focus();
		}

		private static FrameworkElement GetSelection(FrameworkElement activeEllipse, Vector2 direction){
			Func<FrameworkElement, Vector2> getPos = p => new Vector2(
				Canvas.GetLeft(p) + p.ActualWidth  / 2,
				Canvas.GetTop (p) + p.ActualHeight / 2
			);

			var jsAngle = Math.Atan2(direction.X, direction.Y);
			var children = RecursiveChildren<FrameworkElement>(Utils.FindParentOfType<Window>(activeEllipse)).ToArray();
			var activePos = getPos(activeEllipse);

			var allElements = (
				from elm in children
				let elmPos = getPos(elm)
				let diff = new Vector2(
					elmPos.X - activePos.X,
					activePos.Y - elmPos.Y
				)
				let angle = Math.Atan2(diff.X, diff.Y)
				let angleDiff = Math.Abs(jsAngle - angle)

				let diffDist = diff * diff
				let distance = Math.Abs(Math.Sqrt(diffDist.X + diffDist.Y))

				select new {
					Element = elm,
					AngleDiff = angleDiff,
					Distance = distance
				}
			).ToArray();

			var res = (
				from elm in allElements
				where elm.Element != activeEllipse && elm.AngleDiff < Math.PI / 6 // wtf
				orderby Math.Abs(Math.Sin(elm.AngleDiff) * elm.Distance) + Math.Abs(Math.Cos(elm.AngleDiff) * elm.Distance)
				select elm.Element
			).FirstOrDefault();
			return res;
		}

		private static Vector2 Convert(Point p){
			return new Vector2(p.X, p.Y);
		}

		private static IEnumerable<T> RecursiveChildren<T>(DependencyObject depObj)  where T:DependencyObject{
			if (depObj == null) yield break;
			for (var i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(depObj); i++) {
				var child = System.Windows.Media.VisualTreeHelper.GetChild(depObj, i);
				if (child != null)
					yield return (T) child;

				foreach (var childOfChild in RecursiveChildren<T>(child))
					yield return childOfChild;
			}
		}

		private struct ControlPosition {
			private ControlPosition(FrameworkElement elm, Vector2 point)
				: this() {
				Elm = elm;
				Pos = point;
			}
			public readonly FrameworkElement Elm;
			public Vector2 Pos;

			public static IEnumerable<ControlPosition> GetPositions(FrameworkElement elm) {
				var controlAbs = Convert(elm.TransformToAncestor(elm.GetParentWindow()).Transform(new Point(0, 0)));
				var actualWidth = elm.ActualWidth;
				var actualHeight = elm.ActualHeight;

				yield return new ControlPosition(elm, controlAbs);
				yield return new ControlPosition(elm, controlAbs + new Vector2(actualWidth, 0));
				yield return new ControlPosition(elm, controlAbs + new Vector2(0, actualHeight));
				yield return new ControlPosition(elm, controlAbs + new Vector2(actualWidth, actualHeight));

				yield return new ControlPosition(elm, controlAbs + new Vector2(actualWidth / 2, 0));
				yield return new ControlPosition(elm, controlAbs + new Vector2(0, actualHeight / 2));
				yield return new ControlPosition(elm, controlAbs + new Vector2(actualWidth / 2, actualHeight));
				yield return new ControlPosition(elm, controlAbs + new Vector2(actualWidth, actualHeight / 2));
			}
		}
	}
}
