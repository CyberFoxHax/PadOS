using System.Windows;
using System.Linq;
using System.Collections.Generic;

namespace PadOS.Input.BlockNavigator {
    public static partial class BlockNavigator {

        /// <summary>
        /// Recalculates the sizes and positions for all controls in this tree.
        /// It relies on LayoutUpdated therefore it only works once per mutation.
        /// </summary>
        public static async System.Threading.Tasks.Task RefreshLayout(FrameworkElement elm) {
            var nav = GetBlockNavigator(elm);
            var parent = Utils.FindBlockNavigatorElement(elm);

            FrameworkElement GetHiddenParent(FrameworkElement c, System.Func<FrameworkElement, bool> condition) {
                while (true) {
                    if (condition(c))
                        return c;
                    var p = System.Windows.Media.VisualTreeHelper.GetParent(c);
                    if (p!=null)
                        c = (FrameworkElement)p; // TODO: parent is null inside listviews. There should be a wpf utility for tree travesal somewhere
                    else
                        break;
                }
                return null;
            }

            await new Await(parent, _ => {
                if(nav == null)
                    nav = GetBlockNavigator(parent);
                if (_hiddenBlocks.ContainsKey(nav) == false)
                    _hiddenBlocks[nav] = new HashSet<FrameworkElement>();

                foreach (var item in nav._blocks.Keys.Concat(_hiddenBlocks[nav]).ToArray()) {
                    if (GetHiddenParent(item, p=>p.Visibility != Visibility.Visible) != null) {
                        nav._blocks.Remove(item);
                        _hiddenBlocks[nav].Add(item);
                        continue;
                    }

                    var root = GetHiddenParent(item, p => p == nav.OwnerElement);

                    if (GetHiddenParent(item, p => p == nav.OwnerElement) == null) {
                        continue;
                    }

                    var point = item
                        .TransformToAncestor(nav.OwnerElement)
                        .Transform(new Point(0, 0));

                    _hiddenBlocks[nav].Remove(item);
                    nav._blocks[item] = new Rect(
                        point.X,
                        point.Y,
                        item.ActualWidth,
                        item.ActualHeight
                    );
                }
            }).LayoutUpdated().Task();
        }
        private static Dictionary<BlockNavigatorInternal, HashSet<FrameworkElement>> _hiddenBlocks = new Dictionary<BlockNavigatorInternal, HashSet<FrameworkElement>>();

        /// <summary>
        /// Forcefully activate a BlockNavigator
        /// </summary>
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
