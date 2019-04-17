using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BlockNavigatorProperty = PadOS.Input.BlockNavigator.BlockNavigator;

namespace PadOS.Input.BlockNavigator {
    internal static class Utils {
        public static FrameworkElement FindBlockNavigatorElement(FrameworkElement elm) {
            var searchElm = elm;

            // if we query the navigator on a nested element, make sure we get the parent navigator and not the self
            if (BlockNavigatorProperty.GetIsNestedNavigation(searchElm) && searchElm.Parent != null)
                searchElm = (FrameworkElement)searchElm.Parent;

            while (true) {
                if (searchElm.Parent == null)
                    return searchElm;

                var isNestedNavigator = BlockNavigatorProperty.GetIsNestedNavigation(searchElm);
                if (isNestedNavigator)
                    return searchElm;

                searchElm = (FrameworkElement)searchElm.Parent;
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
            nav.ExplicitDisabled = value;
            nav.IsEnabled = !value;
        }
    }
}
