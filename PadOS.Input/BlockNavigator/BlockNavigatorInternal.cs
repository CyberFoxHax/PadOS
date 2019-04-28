using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using BlockNavigatorProperty = PadOS.Input.BlockNavigator.BlockNavigator;

namespace PadOS.Input.BlockNavigator {
	internal partial class BlockNavigatorInternal{
		internal BlockNavigatorInternal(FrameworkElement element) {
			_xInput = new GamePadInput.GamePadInput();
			InitGamepad();
			var frameworkElement = element;
			if (frameworkElement == null) return;
			OwnerElement = frameworkElement;
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

        private FrameworkElement _focusElement;
		private BlockNavigationCursor _cursor;
		private bool _hasManualFocus;
		private readonly Dictionary<FrameworkElement, Rect> _blocks = new Dictionary<FrameworkElement, Rect>();
        public FrameworkElement OwnerElement { get; private set; }
        public BlockNavigatorInternal ParentNavigator { get; set; }
        public bool ExplicitDisabled { get; set; }

        private bool _isEnabled;
        public bool IsEnabled {
            get { return _isEnabled; }
            set {
                if (ExplicitDisabled)
                    return;
                _isEnabled = value;
                OnIsEnabledChanged(value);
            }
        }

        public void SetFocus(FrameworkElement element, bool animate = true) {
            if (_blocks.ContainsKey(element) == false)
                throw new System.Exception("You are attempting to focus an element that is not registered in the BlockNavigator. " +
                    "You are most likely trying to focus a child or a parent.\nElement Type: \"" + element + "\"");
            OnFocusChanged(element, animate);
        }

		private void SetInitialFocus(FrameworkElement elm){
            var initialFocus = BlockNavigatorProperty.GetInitialFocus(elm);

            if (initialFocus) {
                _focusElement = elm;
                _hasManualFocus = true;
                OnFocusChanged(elm, false);
            }
            else if(_hasManualFocus == false && _focusElement == null) {
                _focusElement = elm;
                OnFocusChanged(elm, false);
            }
        }
        
    }
}