using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BlockNavigatorProperty = PadOS.Input.BlockNavigator.BlockNavigator;

namespace PadOS.Input.BlockNavigator {
    internal static class Utils {
        public static FrameworkElement FindBlockNavigatorElement(FrameworkElement elm) {
            var searchElm = elm;

            DependencyObject GetParent(FrameworkElement child) {
                return child.Parent ??
                    child.TemplatedParent ??
                    System.Windows.Media.VisualTreeHelper.GetParent(child);
            }

            // if we query the navigator on a nested element, make sure we get the parent navigator and not the self
            if (BlockNavigatorProperty.GetIsNestedNavigation(searchElm) && GetParent(searchElm) != null)
                searchElm = (FrameworkElement)GetParent(searchElm);

            while (true) {
                var parent = GetParent(searchElm);

                if (parent == null)
                    return searchElm;

                var isNestedNavigator = BlockNavigatorProperty.GetIsNestedNavigation(searchElm);
                if (isNestedNavigator)
                    return searchElm;

                searchElm = (FrameworkElement)parent;
            }
        }

        public static void RegisterNavigationBlock(FrameworkElement frameworkElement) {
            var parentElement = FindBlockNavigatorElement(frameworkElement);
            var nav = BlockNavigatorProperty.GetBlockNavigator(parentElement);
            if (nav == null) {
                nav = CreateNavigator(parentElement);
                nav.IsEnabled = true;
            }

            // bugfix: The control will not be part of the correct hierarchy until it's fully loaded
            frameworkElement.Loaded += delegate {
                nav.OnAddBlock(frameworkElement);
            };
        }

        public static void RegisterNestedNavigationBlock(FrameworkElement frameworkElement) {
            // register on parent navigator that is
            RegisterNavigationBlock(frameworkElement);

            var navigator = BlockNavigatorProperty.GetBlockNavigator(frameworkElement);
            if(navigator == null) {
                navigator = CreateNavigator(frameworkElement);
                navigator.IsEnabled = false;
            }
            var parentNavigator = FindBlockNavigatorElement(frameworkElement);
            navigator.ParentNavigator = BlockNavigatorProperty.GetBlockNavigator(parentNavigator);
        }

        public static BlockNavigatorInternal CreateNavigator(FrameworkElement element) {
            var nav = new BlockNavigatorInternal(element);
            BlockNavigatorProperty.SetBlockNavigator(element, nav);
            return nav;
        }

        public static void OnDisableBlockNavigatorChanged(FrameworkElement a, bool value) {
            var nav = BlockNavigatorProperty.GetBlockNavigator(a);
            if(nav == null) {
                var elm = FindBlockNavigatorElement(a);
                nav = BlockNavigatorProperty.GetBlockNavigator(elm);
            }
            nav.IsEnabled = !value;
            nav.ExplicitDisabled = value;
        }
    }
}
