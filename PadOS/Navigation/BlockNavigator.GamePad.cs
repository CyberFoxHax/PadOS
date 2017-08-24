using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PadOS.Input;
using XInputDotNetPure;

namespace PadOS.Navigation
{
	public partial class BlockNavigator : IDisposable{
		private GamePadInput.GamePadEvent GetDPadEvent(double x, double y) => (a,b)=>OnDPad(new Vector2(x, y));

		private const double ResetThreshold = 0.3;
		private bool _waitForReset;
		private readonly GamePadInput _xInput;

		private void OnDPad(Vector2 vector2) {
			var res = GetSelection(_focusElm, vector2);
			if (res != null)
				SetFocus(res);
		}

		private void OnThumbChange(PlayerIndex player, GamePadState state, Vector2 value) {
			var gamePadState = state;
			var vector = new Vector2(gamePadState.ThumbSticks.Left.X, gamePadState.ThumbSticks.Left.Y);
			var thumbLength = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (_waitForReset && thumbLength > ResetThreshold) return;

			if (thumbLength < ResetThreshold)
				_waitForReset = false;
			else if (_waitForReset)
				return;

			if (thumbLength < ResetThreshold) return;

			var res = GetSelection(_focusElm, new Vector2(
				vector.X,
				vector.Y
			));
			if (res == null) return;
			_waitForReset = true;
			SetFocus(res);
		}

		private FrameworkElement GetSelection(FrameworkElement activeElement, Vector2 direction){

			// algorithm doesn't work with full blocks

			Vector2 GetPos(FrameworkElement elm){
				var block = _blocks[elm];
				return new Vector2(block.X, block.Y) + new Vector2(block.Width, block.Height)/2;
			}

			var jsAngle = Math.Atan2(direction.X, direction.Y);
			var children = _blocks;
			var activePos = GetPos(activeElement);


			var allElements = (
				from elm in children
				let elmPos = elm.Value
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
				where elm.Element.Key != activeElement && elm.AngleDiff < tau / 8
				orderby Math.Abs(Math.Sin(elm.AngleDiff) * elm.Distance) + Math.Abs(Math.Cos(elm.AngleDiff) * elm.Distance)
				select elm.Element
			).FirstOrDefault();

			return res.Key;
		}

		public void Dispose(){
			_xInput.Dispose();
		}
	}
}
