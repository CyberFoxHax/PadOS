using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BlockNavigatorProperty = PadOS.Input.BlockNavigator.BlockNavigator;


namespace PadOS.Input.BlockNavigator {
    internal partial class BlockNavigatorInternal {

        public void OnAddBlock(FrameworkElement elm) {
            if (_blocks.ContainsKey(elm))
                return;
            var navigatorElement = Utils.FindBlockNavigatorElement(elm);
            var point = elm
                .TransformToAncestor(navigatorElement)
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
            if (_focusElement == null)
                SetInitialFocus();
        }

        private void OnSimulateMouse(FrameworkElement _focusElm) {
            _focusElm.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left) {
                RoutedEvent = Mouse.MouseUpEvent,
                Source = this
            });
            if (_focusElm is Button) {
                _focusElm.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, _focusElm));
                // since i don't use "buttons" in my application this remains untested
                // should a test arise in the future, delete this if it succeeds
                System.Diagnostics.Debugger.Break();
            }
        }

        private void OnActivateNestedNavigator(FrameworkElement _focusElm) {
            IsEnabled = false;
            var nav = BlockNavigatorProperty.GetBlockNavigator(_focusElm);
            nav.ParentNavigator = this;
            nav.IsEnabled = true;
        }

        private void OnIsEnabledChanged(bool value) {
            _xInput.IsEnabled = value;
            _waitForReturn = true;
            if (_cursor != null)
                _cursor.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OnFocusChanged(FrameworkElement elm) {
            _focusElement?.Dispatcher.Invoke(() => {
                BlockNavigatorProperty.SetIsFocused(_focusElement, false);
                _focusElement?.RaiseEvent(new RoutedEventArgs(BlockNavigatorProperty.CursorExitEvent, _focusElement));
            });
            elm.Dispatcher.Invoke(() => {
                _focusElement = elm;
                BlockNavigatorProperty.SetIsFocused(_focusElement, true);
                _focusElement.RaiseEvent(new RoutedEventArgs(BlockNavigatorProperty.CursorEnterEvent, _focusElement));
                _cursor.TargetRect = _blocks[_focusElement];
                _cursor.Visibility = BlockNavigatorProperty.GetHideCursor(_focusElement) || IsEnabled == false
                    ? Visibility.Hidden
                    : Visibility.Visible;
            });
        }

        public void OnNavigateBack() {
            if (ParentNavigator == null)
                return;

            IsEnabled = false;
            var nav = ParentNavigator;
            nav.IsEnabled = true;
        }

    }
}
