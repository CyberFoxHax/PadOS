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
		private GamePadEvent GetDPadEvent(double x, double y) => (a,b)=>OnDPad(new Vector2(x, y));

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
			_xInput.IsEnabled = true;
			_waitForReset = true;
		}

		private void OnCancelClick(int player, GamePadState state){
			_focusElm?.Dispatcher.Invoke(() => {
				_focusElm?.RaiseEvent(new RoutedEventArgs(CancelClickEvent, _focusElm));
			});
		}

		private void OnConfirmClick(int player, GamePadState state) {
			_focusElm?.Dispatcher.Invoke(() => {
				_focusElm?.RaiseEvent(new RoutedEventArgs(ConfirmClickEvent, _focusElm));
			});
		}

		private void OnDPad(Vector2 vector2) {
			var res = GetSelection(_focusElm, vector2);
			if (res != null)
				SetFocus(res);
		}

		private void OnThumbChange(int i, GamePadState state, Vector2 value) {
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
			var activeBlock = _blocks[activeElement];
			var blocks = _blocks;

			var angle = direction.GetAngle()+Math.PI;

			const double tau = Math.PI*2;
			const double segmentSize = tau / 8;
			const int intersectionPadding = 1;

			if (angle > segmentSize * 5 && angle < segmentSize * 7) {
				// right
				return (
					from block in blocks
					let rect = block.Value
					where ReferenceEquals(block.Key, activeElement) == false
					&& rect.Left >= activeBlock.Right
					orderby rect.Left
					let overlap = new Rect(
						activeBlock.Right+intersectionPadding,
						activeBlock.Top+intersectionPadding,
						Math.Abs(rect.Right - activeBlock.Right)-intersectionPadding*2,
						activeBlock.Height-intersectionPadding*2
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
					&& rect.Right <= activeBlock.Left
					orderby rect.Right descending
					let overlap = new Rect(
						rect.Left+intersectionPadding,
						activeBlock.Top+intersectionPadding,
						Math.Abs(activeBlock.Left - rect.Left)-intersectionPadding*2,
						activeBlock.Height-intersectionPadding*2
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
					&& rect.Top >= activeBlock.Bottom
					orderby rect.Top
					let overlap = new Rect(
						activeBlock.Left+intersectionPadding,
						activeBlock.Bottom+intersectionPadding,
						activeBlock.Width-intersectionPadding*2,
						Math.Abs(rect.Bottom - activeBlock.Bottom)-intersectionPadding*2
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
					&& rect.Bottom <= activeBlock.Top
					orderby rect.Bottom descending
					let overlap = new Rect(
						activeBlock.Left+intersectionPadding,
						rect.Top+intersectionPadding,
						activeBlock.Width-intersectionPadding*2,
						Math.Abs(rect.Top - activeBlock.Top)-intersectionPadding*2
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
