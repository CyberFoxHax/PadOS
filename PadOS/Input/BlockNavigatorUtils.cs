using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace PadOS.Input {
    public static class BlockNavigatorUtils {
        private static FrameworkElement FindNavigatorElement(FrameworkElement elm, BlockNavigator self = null) {
            var searchElm = elm;
            while (true) {
                if (searchElm.Parent == null)
                    break;
                if ((bool)searchElm.GetValue(BlockNavigator.NestedNavigationProperty) == true) {
                    var nav = BlockNavigator.GetBlockNavigator(searchElm);
                    if (self != null && nav != self) {
                        searchElm = (FrameworkElement)searchElm.Parent;
                        continue;
                    }
                    return searchElm;
                }
                searchElm = (FrameworkElement)searchElm.Parent;
            }
            return searchElm;
        }

        private static void RegisterNavigationBlock(DependencyObject registererdControl) {
            var frameworkElement = registererdControl as FrameworkElement;
            if (frameworkElement == null)
                return;

            var parent = FindNavigatorElement(frameworkElement);
            var nav = GetBlockNavigator(parent);
            if (nav == null)
                nav = CreateNavigatorAtRoot(parent);
            // bugfix: The control will not be part of the correct hierarchy until it's fully loaded
            frameworkElement.Loaded += delegate {
                nav.AddBlock(frameworkElement);
            };
        }

        private static void RegisterNestedNavigationBlock(DependencyObject registererdControl) {
            var frameworkElement = registererdControl as FrameworkElement;
            if (frameworkElement == null)
                return;


        }

        private static void RegisteredChanged(DependencyObject registererdControl) {
            var frameworkElement = registererdControl as FrameworkElement;
            if (frameworkElement == null)
                return;
            var parent = FindNavigatorElement(frameworkElement);
            if (parent == registererdControl && frameworkElement.Parent != null)
                parent = FindNavigatorElement((FrameworkElement)frameworkElement.Parent);
            var nav = GetBlockNavigator(parent);
            if (nav == null) {

            }
            if (GetNestedNavigation(frameworkElement) == true) {
                var nestedNavigator = GetBlockNavigator(frameworkElement);
                nestedNavigator._parentNavigator = nav;
                nestedNavigator.IsEnabled = false;
            }
            // bugfix: The control will not be part of the correct hierarchy until it's fully loaded
            frameworkElement.Loaded += delegate {
                nav.AddBlock(frameworkElement);
            };
        }

        private static BlockNavigator CreateNavigatorAtRoot(DependencyObject child) {
            var frameworkElement = child as FrameworkElement;
            if (frameworkElement == null)
                return;

            var top = (UIElement)frameworkElement.FindRootElement();
            SetBlockNavigator(top);
            var nav = GetBlockNavigator(top);
            nav.IsEnabled = false;
            return nav;
        }


    }
}
