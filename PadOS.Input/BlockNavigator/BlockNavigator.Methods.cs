using System.Windows;
using System.Linq;

namespace PadOS.Input.BlockNavigator {
    public static partial class BlockNavigator {

        public static async System.Threading.Tasks.Task RefreshLayout(FrameworkElement elm) {
            var parent = Utils.FindBlockNavigatorElement(elm);

            await new Await(parent, p=> {
                var nav = GetBlockNavigator(parent);
                foreach (var item in nav._blocks.Keys.ToArray()) {
                    var point = item
                        .TransformToAncestor(parent)
                        .Transform(new Point(0, 0));

                    nav._blocks[item] = new Rect(
                        point.X,
                        point.Y,
                        item.ActualWidth,
                        item.ActualHeight
                    );
                }
            }).LayoutUpdated().Task();
        }

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
        /// Will change the focus to another control. If you focus a control in a seperate tree, use the overload.
        /// </summary>
        public static void SetFocus(FrameworkElement newElement, bool animate = false) {
            {
                var parent = Utils.FindBlockNavigatorElement(newElement);
                var nav = GetBlockNavigator(parent);
                nav.SetFocus(newElement, animate);
            }
        }

        /// <summary>
        /// Will change the focus to another control. If the control resides within another navigation tree,
        /// Then current tree will be disabled and the new tree will be activated
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
