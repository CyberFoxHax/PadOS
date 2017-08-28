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

		private bool _aIsConfirm = true;

		private const double ResetThreshold = 0.3;
		private bool _waitForReset;
		private readonly GamePadInput _xInput;

		private void InitGamepad(){
			_xInput.ThumbLeftChange += OnThumbChange;
			_xInput.DPadDownDown	+= GetDPadEvent( 0, -1);
			_xInput.DPadUpDown		+= GetDPadEvent( 0,  1);
			_xInput.DPadLeftDown	+= GetDPadEvent(-1,  0);
			_xInput.DPadRightDown	+= GetDPadEvent( 1,  0);
			if(_aIsConfirm) {
				_xInput.ButtonADown += OnConfirmClick;
				_xInput.ButtonBDown += OnCancelClick;
			}
			else {
				_xInput.ButtonADown += OnCancelClick;
				_xInput.ButtonBDown += OnConfirmClick;
			}
			_xInput.Enable();
		}

		private void OnCancelClick(PlayerIndex player, GamePadState state){
			_focusElm.Dispatcher.Invoke(() => {
				_focusElm?.RaiseEvent(new RoutedEventArgs(CancelClickEvent, _focusElm));
			});
		}

		private void OnConfirmClick(PlayerIndex player, GamePadState state) {
			_focusElm.Dispatcher.Invoke(() => {
				_focusElm?.RaiseEvent(new RoutedEventArgs(ConfirmClickEvent, _focusElm));
			});
		}

		private void OnDPad(Vector2 vector2) {
			var res = GetSelection(_focusElm, vector2);
			if (res != null)
				SetFocus(res);
		}

		private void OnThumbChange(PlayerIndex player, GamePadState state, Vector2 value) {
			var gamePadState = state;
			var vector = new Vector2(gamePadState.ThumbSticks.Left.X, gamePadState.ThumbSticks.Left.Y);
			var thumbLength = vector.GetLength();
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
			var baseBlock = _blocks[activeElement];
			var blocks = _blocks;

			var angle = direction.GetAngle()+Math.PI;

			const double tau = Math.PI*2;
			const double segmentSize = tau / 8;

			if (angle > segmentSize * 5 && angle < segmentSize * 7) {
				// right
				return (
					from block in blocks
					let rect = block.Value
					where ReferenceEquals(block.Key, activeElement) == false
					&& rect.Left >= baseBlock.Right
					orderby rect.Left
					let overlap = new Rect(
						baseBlock.Right+1,
						baseBlock.Top+1,
						Math.Abs(rect.Right - baseBlock.Right)-1,
						baseBlock.Height-1
					)
					where	overlap.IntersectsWith(rect)
					select block
				).FirstOrDefault().Key;
			}
			if (angle > segmentSize * 1 && angle < segmentSize * 3) {
				// left
				return (
					from block in blocks
					let rect = block.Value
					where ReferenceEquals(block.Key, activeElement) == false
					&& rect.Right <= baseBlock.Left
					orderby rect.Right
					let overlap = new Rect(
						rect.Left+1,
						baseBlock.Top+1,
						Math.Abs(baseBlock.Left - rect.Left)-1,
						baseBlock.Height-1
					)
					where overlap.IntersectsWith(rect)
					select block
				).FirstOrDefault().Key;
			}
			if (angle >= segmentSize * 0 && angle <  segmentSize * 1
			||	angle >  segmentSize * 7 && angle <= segmentSize * 8) {
				// down
				return (
					from block in blocks
					let rect = block.Value
					where ReferenceEquals(block.Key, activeElement) == false
					&& rect.Top >= baseBlock.Bottom
					orderby rect.Top
					let overlap = new Rect(
						baseBlock.Left+1,
						baseBlock.Bottom+1,
						baseBlock.Width-1,
						Math.Abs(rect.Bottom - baseBlock.Bottom)-1
					)
					where overlap.IntersectsWith(rect)
					select block
				).FirstOrDefault().Key;
			}
			if (angle > segmentSize * 3 && angle < segmentSize * 5) {
				// up
				return (
					from block in blocks
					let rect = block.Value
					where ReferenceEquals(block.Key, activeElement) == false
					&& rect.Bottom <= baseBlock.Top
					orderby rect.Bottom
					let overlap = new Rect(
						baseBlock.Left+1,
						rect.Top+1,
						baseBlock.Width-1,
						Math.Abs(rect.Top - baseBlock.Top)-1
					)
					where overlap.IntersectsWith(rect)
					select block
				).FirstOrDefault().Key;
			}

			return null;
		}

		public void Dispose(){
			_xInput.Dispose();
		}
	}
}
