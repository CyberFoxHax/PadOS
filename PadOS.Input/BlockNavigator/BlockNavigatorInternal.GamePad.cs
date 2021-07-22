using System;
using System.Linq;
using System.Windows;
using XInputDotNetPure;
using BlockNavigatorProperty = PadOS.Input.BlockNavigator.BlockNavigator;


namespace PadOS.Input.BlockNavigator {
	internal partial class BlockNavigatorInternal : IDisposable{
		private GamePadEvent GetDPadEvent(double x, double y) => (a,b)=>OnDPad(new Vector2(x, y));

		private bool _aIsConfirm = true;

		private const double MovementThreshold = 0.1;
		private bool _waitForReturn;
		private bool _waitForInnerReturn;
		private readonly GamePadInput.GamePadInput _xInput;
        private double _lowThumbLength;
        private double _highThumbLength;
        private bool _firstTime = true;

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
			_waitForReturn = false;
		}

		private void OnCancelClick(int player, GamePadState state){
			_focusElement?.Dispatcher.Invoke(() => {
                OwnerElement?.RaiseEvent(new RoutedEventArgs(BlockNavigatorProperty.CancelClickEvent, OwnerElement));
                if(OwnerElement != _focusElement)
                    _focusElement?.RaiseEvent(new RoutedEventArgs(BlockNavigatorProperty.CancelClickEvent, _focusElement));
                NavigateBack();
            });
		}

		private void OnConfirmClick(int player, GamePadState state) {
            _focusElement?.Dispatcher.Invoke(() => {
                OwnerElement?.RaiseEvent(new RoutedEventArgs(BlockNavigatorProperty.ConfirmClickEvent, OwnerElement));
                if(OwnerElement != _focusElement)
                    _focusElement?.RaiseEvent(new RoutedEventArgs(BlockNavigatorProperty.ConfirmClickEvent, _focusElement));
                if (BlockNavigatorProperty.GetSimulateMouse(_focusElement))
                    SimulateMouse(_focusElement);

                if (BlockNavigatorProperty.GetIsNestedNavigation(_focusElement))
                    ActivateNestedNavigator(_focusElement);
            });
		}

        private void OnDPad(Vector2 vector2) {
			var res = GetSelection(_focusElement, vector2);
			if (res != null)
				OnFocusChanged(res);
		}

        // when navigating in one direction multiple it is easier to jiggle the thumbstick, this alsoritm allows
        // you to jiggle it in one direction, and for every outward motion that exceeds a delta of 0.1 the nav will go next
        private bool ShouldNavigate(GamePadState state) {
            var vector = new Vector2(
                state.ThumbSticks.Left.X,
                state.ThumbSticks.Left.Y
            );
            var thumbLength = vector.GetLength();

            if(_firstTime) {
                _firstTime = false;
                if (thumbLength > MovementThreshold)
                    return false;
            }

            if (thumbLength < MovementThreshold) {
                _lowThumbLength = 1;
                _highThumbLength = 0;
                _waitForReturn = false;
                _waitForInnerReturn = false;
                return false;
            }

            if (_waitForReturn) {
                if (_highThumbLength < thumbLength)
                    _highThumbLength = thumbLength;
                var highDiff = thumbLength - _highThumbLength;

                if (_waitForInnerReturn) {
                    if (thumbLength < _lowThumbLength)
                        _lowThumbLength = thumbLength;
                    var lowDiff  = thumbLength - _lowThumbLength;

                    if (lowDiff > MovementThreshold) {
                        _waitForInnerReturn = false;
                        _highThumbLength = _lowThumbLength;
                        _lowThumbLength = 1;
                        return true;
                    }

                }
                else if(highDiff < -MovementThreshold) 
                    _waitForInnerReturn = true;
            }
            else 
                if(thumbLength > MovementThreshold) {
                    _waitForReturn = true;
                    return true;
                }

            return false;
        }

		private void OnThumbChange(int i, GamePadState state, Vector2 value) {
            if (_blocks.Count == 0)
                return;

            if (ShouldNavigate(state) == false)
                return;

			var res = GetSelection(_focusElement, new Vector2(
                state.ThumbSticks.Left.X,
                state.ThumbSticks.Left.Y
            ));
			if (res == null) return;
			_waitForReturn = true;
			OnFocusChanged(res);
		}


        // TODO: This algo is fucking shit
        // Purpose. Determine which element should be selected given the thumbstick direction
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
					&& rect.Top+5 >= activeBlock.Bottom
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
					&& rect.Bottom-5 <= activeBlock.Top
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
