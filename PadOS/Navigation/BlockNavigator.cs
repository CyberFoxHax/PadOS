using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using PadOS.Input;

namespace PadOS.Navigation{
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

        private bool _isEnabled;
        public bool IsEnabled {
            get { return _isEnabled; }
            set {
                _isEnabled = value;
                _xInput.IsEnabled = value;
                if(_cursor!=null)
                    _cursor.IsEnabled = value; 
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

		private static FrameworkElement FindNavigatorElement(FrameworkElement elm, BlockNavigator self=null) {
			var searchElm = elm;
            while (true) {
                if (searchElm.Parent == null)
                    break;
                if ((bool)searchElm.GetValue(NestedNavigationProperty) == true) {
                    var nav = GetBlockNavigator(searchElm);
                    if (self != null && nav != self) {
                        searchElm = (FrameworkElement)searchElm.Parent;
                        continue;
                    }
                }
                searchElm = (FrameworkElement)searchElm.Parent;
            }
			return searchElm;
		}

		public void AddBlock(FrameworkElement elm){
			if (_blocks.ContainsKey(elm)) return;
			var point = elm
				.TransformToAncestor(FindNavigatorElement(elm, this))
				.Transform(new Point(0, 0));
			_blocks.Add(
				elm,
				new Rect(
					point.X,
					point.Y,
					elm.ActualWidth,
					elm.ActualHeight
				)
			);
			if (_focusElm == null)
				SetInitialFocus();
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

		private static void RegisteredChanged(DependencyObject registererdControl) {
			var frameworkElement = registererdControl as FrameworkElement;
			if (frameworkElement == null) return;

			var parent = FindNavigatorElement(frameworkElement);
            if(parent == registererdControl && frameworkElement.Parent != null)
                parent = FindNavigatorElement((FrameworkElement)frameworkElement.Parent);
			var nav = GetBlockNavigator(parent);
            if (nav == null) {
                var top = (UIElement)frameworkElement.FindRootElement();
                SetBlockNavigator(top);
                nav = GetBlockNavigator(top);
                nav.IsEnabled = true;
            }
            if (GetNestedNavigation(frameworkElement) == true) {
                var nestedNavigator = GetBlockNavigator(frameworkElement);
                nestedNavigator.IsEnabled = false;
            }
            // bugfix: The control will not be part of the correct hierarchy until it's fully loaded
			frameworkElement.Loaded += delegate {
				nav.AddBlock(frameworkElement);
			};
		}

        private void OnSimulateMouse(FrameworkElement _focusElm) {
            _focusElm.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left) {
                RoutedEvent = Mouse.MouseUpEvent,
                Source = this
            });
            if (_focusElm is Button) {
                _focusElm.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, _focusElm));
                // since i don't use buttons in my application this remains untested
                // should a test arise in the future, delete this if it succeeds
                System.Diagnostics.Debugger.Break();
            }
        }

        private void OnActivateNestedNavigator(FrameworkElement _focusElm) {

        }
    }
}