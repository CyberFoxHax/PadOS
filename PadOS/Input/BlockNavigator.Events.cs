using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace PadOS.Input {
    public partial class BlockNavigator {

        public void OnAddBlock(FrameworkElement elm) {
            if (_blocks.ContainsKey(elm))
                return;
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
            var nav = GetBlockNavigator(_focusElm);
            nav._parentNavigator = this;
            nav.IsEnabled = true;
        }

        private void OnNavigateBack() {
            if (_parentNavigator == null)
                return;

            IsEnabled = false;
            var nav = _parentNavigator;
            nav.IsEnabled = true;
        }

    }
}
