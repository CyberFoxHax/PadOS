using System.Windows;

namespace PadOS.Input.BlockNavigator {
    public static partial class BlockNavigator {

        public static void EnterNestedNavigator(FrameworkElement element) {
            var parent = Utils.FindBlockNavigatorElement(element);
            var nav = GetBlockNavigator(parent);
            nav.ActivateNestedNavigator(element);
        }

        /// <summary>
        /// Trigger back navigation manually
        /// </summary>
        public static void NavigateBack(FrameworkElement element) {
            var parent = Utils.FindBlockNavigatorElement(element);
            var nav = GetBlockNavigator(parent);
            nav.NavigateBack();
        }


        /// <summary>
        /// Will change the focus to another control. If the control resides within another navigation tree, Then current tree will be disabled and the new tree will be activated
        /// </summary>
        public static void SetFocus(FrameworkElement currentElement, FrameworkElement newElement, bool animate = false) {
            {
                var parent = Utils.FindBlockNavigatorElement(currentElement);
                var nav = GetBlockNavigator(parent);
                nav.IsEnabled = false;
            }
            {
                var parent = Utils.FindBlockNavigatorElement(newElement);
                var nav = GetBlockNavigator(parent);
                nav.IsEnabled = true;
                nav.SetFocus(newElement, animate);
            }
        }

    }
}
