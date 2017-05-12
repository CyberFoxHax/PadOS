﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PadOS.Input {
	public class WPFDirectionalControls{

		public static WPFDirectionalControls Register(IGamePadFocusable control) {
			var dirControl = new WPFDirectionalControls();
			var container = WpfGamepad.GetInstance(control);
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

		private void OnThumbChange(object sender, WpfGamepad.GamePadEventArgs<Vector2> args){
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
			Func<FrameworkElement, Vector2> getPos = elm => GetControlPosition(elm) + new Vector2(elm.ActualWidth, elm.ActualHeight) / 2;

			var jsAngle = Math.Atan2(direction.X, direction.Y);
			var parentWindow = activeElement.FindParentOfType<Window>();
			var children = RecursiveChildren<FrameworkElement>(parentWindow).Where(p=>p.Focusable).ToArray();
			var activePos = getPos(activeElement);


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
					AngleDiff = Math.Acos(Math.Cos(jsAngle) * Math.Cos(angle) + Math.Sin(jsAngle) * Math.Sin(angle)),
					Distance = distance
				}
			).ToArray();

			const double tau = Math.PI * 2;

			var res = (
				from elm in allElements
				where elm.Element != activeElement && elm.AngleDiff < tau / 8
				orderby Math.Abs(Math.Sin(elm.AngleDiff) * elm.Distance) + Math.Abs(Math.Cos(elm.AngleDiff) * elm.Distance)
				select elm.Element
			).FirstOrDefault();

			return res;
		}

		public static Vector2 GetControlPosition(FrameworkElement elm){
			var parent = elm.FindParentOfType<Window>();
			var point = elm.TransformToAncestor(parent).Transform(new Point(0, 0));
			return new Vector2(point.X, point.Y);
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
	}
}
