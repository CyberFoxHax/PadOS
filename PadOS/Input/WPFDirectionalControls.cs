using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;

namespace PadOS.Input {
	public class WPFDirectionalControls{

		public static WPFDirectionalControls Register(UIElement control) {
			var dirControl = new WPFDirectionalControls();
			var container = WPFGamepad.Register(control);
			container.ThumbLeftChange += dirControl.OnThumbChange;
			container.DPadDownDown	+= (sender, args) => dirControl.OnDPad(sender, new Vector2(0, -1));
			container.DPadUpDown	+= (sender, args) => dirControl.OnDPad(sender, new Vector2(0,  1));
			container.DPadLeftDown	+= (sender, args) => dirControl.OnDPad(sender, new Vector2(-1, 0));
			container.DPadRightDown	+= (sender, args) => dirControl.OnDPad(sender, new Vector2( 1, 0));
			return dirControl;
		}

		private const double ResetThreshold = 0.3;
		private bool _waitForReset;

		private void OnDPad(object sender, Vector2 vector2){
			var elm = sender as FrameworkElement;
			if (elm == null) return;

			var res = GetSelection(elm, vector2);
			if (res != null)
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

		private static FrameworkElement GetSelection(FrameworkElement activeElement, Vector2 direction){
			Func<FrameworkElement, Vector2> getPos = elm => ElementPosition.GetControlPosition(elm) + new Vector2(elm.ActualWidth, elm.ActualHeight) / 2;

			var jsAngle = Math.Atan2(direction.X, direction.Y);
			var parentWindow = activeElement.FindParentOfType<Window>();
			var children = RecursiveChildren<FrameworkElement>(parentWindow).Where(p=>p.Focusable).ToArray();
			var activePos = getPos(activeElement);


			var allElements = (
				from child in children
				let elmPos = getPos(child)

				let diff = new Vector2(
					elmPos.X - activePos.X,
					activePos.Y - elmPos.Y
				)
				let angle = Math.Atan2(diff.X, diff.Y)
				let angleDiff = Math.Abs(jsAngle - angle)

				let diffSquared = diff * diff
				let distance = Math.Abs(Math.Sqrt(diffSquared.X + diffSquared.Y))

				select new {
					Element = child,
					AngleDiff = angleDiff,
					Distance = distance
				}
			).ToArray();

			const double tau = Math.PI * 2;

			var res = (
				from elm in allElements
				where elm.Element != activeElement && elm.AngleDiff < tau / 10 // tight angle
				orderby Math.Abs(Math.Sin(elm.AngleDiff) * elm.Distance) + Math.Abs(Math.Cos(elm.AngleDiff) * elm.Distance)
				select elm.Element
			).FirstOrDefault() ?? (
				from elm in allElements
				where elm.Element != activeElement && elm.AngleDiff < tau / 3 // loose angle
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

		private struct ElementPosition {
			private ElementPosition(FrameworkElement elm, Vector2 point)
				: this() {
				Elm = elm;
				Pos = point;
			}
			public readonly FrameworkElement Elm;
			public Vector2 Pos;

			public static Vector2 GetControlPosition(FrameworkElement elm) {
				return GetControlPosition(elm.FindParentOfType<Window>(), elm);
			}

			public static Vector2 GetControlPosition(System.Windows.Media.Visual parent, System.Windows.Media.Visual elm) {
				return Convert(elm.TransformToAncestor(parent).Transform(new Point(0, 0)));
			}

			public static IEnumerable<ElementPosition> GetRectPositions(FrameworkElement elm){
				return GetRectPositions(elm.FindParentOfType<Window>(), elm);
			}

			public static IEnumerable<ElementPosition> GetRectPositions(System.Windows.Media.Visual parent, FrameworkElement elm) {
				var controlAbs = GetControlPosition(parent, elm);
				var actualWidth = elm.ActualWidth;
				var actualHeight = elm.ActualHeight;
				var actualWidth2 = actualWidth;
				var actualHeight2 = actualHeight;

				yield return new ElementPosition(elm, controlAbs);
				yield return new ElementPosition(elm, controlAbs + new Vector2(actualWidth, 0));
				yield return new ElementPosition(elm, controlAbs + new Vector2(0, actualHeight));
				yield return new ElementPosition(elm, controlAbs + new Vector2(actualWidth, actualHeight));

				yield return new ElementPosition(elm, controlAbs + new Vector2(actualWidth2, 0));
				yield return new ElementPosition(elm, controlAbs + new Vector2(0, actualHeight2));
				yield return new ElementPosition(elm, controlAbs + new Vector2(actualWidth2, actualHeight));
				yield return new ElementPosition(elm, controlAbs + new Vector2(actualWidth, actualHeight2));
			}
		}
	}
}
