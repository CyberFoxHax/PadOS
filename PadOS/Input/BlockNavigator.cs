using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace PadOS.Input{
	public partial class BlockNavigator{
		private BlockNavigator(UIElement element) {
			_xInput = new GamePadInput();
			InitGamepad();
			var frameworkElement = (FrameworkElement)element;
			if (frameworkElement == null) return;
			_ownerElement = frameworkElement;
            {
                if (frameworkElement is Window window)
                    window.Closed += delegate { Dispose(); };
            }
            AdornerLayer GetAdornerLayer(Visual visual) {
                var decorator = visual as AdornerDecorator;
                if (decorator != null)
                    return decorator.AdornerLayer;
                var presenter = visual as ScrollContentPresenter;
                if (presenter != null)
                    return presenter.AdornerLayer;
                var window = visual as Window;
                var visualContent = window?.Content as Visual;
                return AdornerLayer.GetAdornerLayer(visualContent ?? visual);
            }
			frameworkElement.Loaded += delegate{
                var layer = GetAdornerLayer(element);
				var child = (element as Window)?.Content as Visual ?? element;
				_cursor = new BlockNavigationCursor((UIElement) child);
                _cursor.Visibility = IsEnabled ? Visibility.Visible : Visibility.Collapsed;
				layer.Add(_cursor);
			};
		}

        private FrameworkElement _focusElm;
		private BlockNavigationCursor _cursor;
		private bool _manualInitialFocusFound;
		private readonly Dictionary<FrameworkElement, Rect> _blocks = new Dictionary<FrameworkElement, Rect>();
		private readonly FrameworkElement _ownerElement;
        private BlockNavigator _parentNavigator;

        private bool _isEnabled;
        public bool IsEnabled {
            get { return _isEnabled; }
            set {
                _isEnabled = value;
                _xInput.IsEnabled = value;
                if(_cursor!=null)
                    _cursor.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

		private void SetFocus(FrameworkElement elm){
			_focusElm?.Dispatcher.Invoke(() => {
				SetIsFocused(_focusElm, false);
				_focusElm?.RaiseEvent(new RoutedEventArgs(CursorExitEvent, _focusElm));
			});
			elm.Dispatcher.Invoke(() => {
				_focusElm = elm;
				SetIsFocused(_focusElm, true);
				_focusElm.RaiseEvent(new RoutedEventArgs(CursorEnterEvent, _focusElm));
				_cursor.TargetRect = _blocks[_focusElm];
				_cursor.Visibility = GetHideCursor(_focusElm) || IsEnabled == false
					? Visibility.Hidden
					: Visibility.Visible;
			});
		}

		

		private void SetInitialFocus(){
			foreach (var block in _blocks) {
				if (GetInitialFocus(block.Key) == false)
					continue;
				_focusElm = block.Key;
				_manualInitialFocusFound = true;
				SetFocus(_focusElm);
				return;
			}

            // if user focus not set, set first element as focus.
            if (_manualInitialFocusFound == false && _focusElm == null) {
                SetFocus(_blocks.First().Key);
            }
        }
        
    }
}