using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using XInputDotNetPure;

namespace PadOS.Input {
	public static class WPFDirectionalControls{

		public static void Register(UIElement control){
			WPFGamepad.Register(control).GamePadOnChange += OnButton;
		}

		private static void OnButton(object sender, GamePadState gamePadState){
			var elm = sender as UIElement;
			if (elm == null) return;

			if		(gamePadState.DPad.Left	== ButtonState.Pressed) OnDirection(elm, new Vector2(-1));
			else if (gamePadState.DPad.Right== ButtonState.Pressed) OnDirection(elm, new Vector2( 1));
			else if (gamePadState.DPad.Up	== ButtonState.Pressed) OnDirection(elm, new Vector2( 0, -1));
			else if (gamePadState.DPad.Down == ButtonState.Pressed) OnDirection(elm, new Vector2( 0,  1));
			else{
				const float threshhold = 0.5f;
				if (Math.Abs(gamePadState.ThumbSticks.Left.X) > threshhold || Math.Abs(gamePadState.ThumbSticks.Left.Y) > threshhold) {// todo this is not circular
					OnDirection(elm, new Vector2(
						 gamePadState.ThumbSticks.Left.X,
						-gamePadState.ThumbSticks.Left.Y
					));
				}
			}
		}

		private struct ControlPosition{
			private ControlPosition(FrameworkElement elm, Vector2 point):this(){
				Elm = elm;
				Pos = point;
			}
			public readonly FrameworkElement Elm;
			public Vector2 Pos;

			public static IEnumerable<ControlPosition> GetPositions(FrameworkElement elm){
				var controlAbs = Convert(elm.TransformToAncestor(elm.GetParentWindow()).Transform(new Point(0, 0)));
				yield return new ControlPosition(elm, controlAbs);
				yield return new ControlPosition(elm, controlAbs + new Vector2(elm.ActualWidth, 0			   ));
				yield return new ControlPosition(elm, controlAbs + new Vector2(0			  , elm.ActualHeight));
				yield return new ControlPosition(elm, controlAbs + new Vector2(elm.ActualWidth, elm.ActualHeight));
			}
		}

		private static void OnDirection(UIElement control, Vector2 direction){
			// from p points
			// where 45 deg
			// orderby angle *** analogous angle
			// orderby dist
			// select first

			var angle = Math.Atan2(direction.X, direction.Y);
			var center = ControlPosition.GetPositions((FrameworkElement)control).ToArray();

			//var result = (
			//	from FrameworkElement child in RecursiveChildren(((FrameworkElement)control).GetParentWindow())
			//	where child.Focusable
			//	from point in ControlPosition.GetPositions(child)
			//	from centerPositions in center
			//	let angleBetweenV = Math.Atan2(
			//		centerPositions.Pos.X - point.Pos.X,
			//		centerPositions.Pos.Y - point.Pos.Y
			//	)
			//	let angleBetween = Math.Abs(angle - angleBetweenV)
			//	where angleBetween < 45
			//	orderby point.Pos.X * point.Pos.X + point.Pos.Y * point.Pos.Y
			//	select point.Elm
			//).FirstOrDefault();

			var result = (
				from FrameworkElement child in RecursiveChildren(((FrameworkElement)control).GetParentWindow())
				where child.Focusable
				from point in ControlPosition.GetPositions(child)
				from centerPositions in center
				let vecDiff = new Vector2(
					centerPositions.Pos.X - point.Pos.X,
					centerPositions.Pos.Y - point.Pos.Y
				)
				//where vecDiff < 45
				orderby point.Pos.X * point.Pos.X + point.Pos.Y * point.Pos.Y
				select point.Elm
			).FirstOrDefault();

			if (result != null){
				((Button)result).Background = System.Windows.Media.Brushes.GreenYellow;
			}
		}

		private static Vector2 Convert(Point p){
			return new Vector2(p.X, p.Y);
		}

		private static IEnumerable<DependencyObject> RecursiveChildren(DependencyObject depObj){
			if (depObj == null) yield break;
			for (var i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(depObj); i++) {
				var child = System.Windows.Media.VisualTreeHelper.GetChild(depObj, i);
				if (child != null)
					yield return child;

				foreach (var childOfChild in RecursiveChildren(child))
					yield return childOfChild;
			}
		}
	}
}
