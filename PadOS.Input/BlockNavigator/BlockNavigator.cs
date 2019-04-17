using System;
using System.Windows;
using ElementType = System.Windows.FrameworkElement;

namespace PadOS.Input.BlockNavigator {
    public static class BlockNavigator{

	    public delegate void NavigationEvent(object sender, EventArgs args);

        private static void RegisterEvent(RoutedEvent evt, FrameworkElement d, NavigationEvent handler) {
            (d as UIElement).AddHandler(evt, handler);
            Utils.RegisterNavigationBlock(d);
        }

        private static void UnregisterEvent(RoutedEvent evt, FrameworkElement d, NavigationEvent handler) {
            (d as UIElement).RemoveHandler(CursorEnterEvent, handler);
            Utils.RegisterNavigationBlock(d);
        }

        /// <summary>
        /// When the cursor hovers the Element. "Sender" is the Element that contains the event
        /// </summary>
	    public static readonly RoutedEvent CursorEnterEvent = EventManager.RegisterRoutedEvent(
		    "CursorEnter",
            RoutingStrategy.Direct,
            typeof(NavigationEvent),
            typeof(BlockNavigator)
        );

        public static void AddCursorEnterHandler(FrameworkElement d, NavigationEvent handler) {
            RegisterEvent(CursorEnterEvent, d, handler);;
        }

        public static void RemoveCursorEnterHandler(FrameworkElement d, NavigationEvent handler) {
            UnregisterEvent(CursorEnterEvent, d, handler);;
        }

        /// <summary>
        /// When the cursor's hover leaves the Element. "Sender" is the Element that contains the event
        /// </summary>
        public static readonly RoutedEvent CursorExitEvent = EventManager.RegisterRoutedEvent(
		    "CursorExit",
            RoutingStrategy.Direct,
            typeof(NavigationEvent),
            typeof(BlockNavigator)
        );

        public static void AddCursorExitHandler(FrameworkElement d, NavigationEvent handler) {
            RegisterEvent(CursorExitEvent, d, handler);;
        }

        public static void RemoveCursorExitHandler(FrameworkElement d, NavigationEvent handler) {
            UnregisterEvent(CursorExitEvent, d, handler);;
        }

        /// <summary>
        /// Confirm button event. Mapped to XInput:A. "Sender" is the Element that contains the event
        /// </summary>
        public static readonly RoutedEvent ConfirmClickEvent = EventManager.RegisterRoutedEvent(
			"ConfirmClick",
            RoutingStrategy.Direct,
            typeof(NavigationEvent),
            typeof(BlockNavigator)
        );

        public static void AddConfirmClickHandler(FrameworkElement d, NavigationEvent handler) {
            RegisterEvent(ConfirmClickEvent, d, handler);;
        }

        public static void RemoveConfirmClickHandler(FrameworkElement d, NavigationEvent handler) {
            UnregisterEvent(ConfirmClickEvent, d, handler);;
        }

        /// <summary>
        /// Cancel button event. Mapped to XInput:B. "Sender" is the Element that contains the event
        /// </summary>
        public static readonly RoutedEvent CancelClickEvent = EventManager.RegisterRoutedEvent(
		    "CancelClick",
            RoutingStrategy.Direct,
            typeof(NavigationEvent),
            typeof(BlockNavigator)
        );

        public static void AddCancelClickHandler(FrameworkElement d, NavigationEvent handler) {
            RegisterEvent(CancelClickEvent, d, handler);;
        }

        public static void RemoveCancelClickHandler(FrameworkElement d, NavigationEvent handler) {
            UnregisterEvent(CancelClickEvent, d, handler);;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Runtime property determining whether the control current is focussed or not (Readonly)
        /// </summary>
        public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached(
		    "IsFocused",
            typeof(bool),
            typeof(BlockNavigator),
            new PropertyMetadata(default(bool))
        );

        internal static void SetIsFocused(FrameworkElement element, bool value) {
            element.SetValue(IsFocusedProperty, value);
        }

        public static bool GetIsFocused(FrameworkElement element) {
            return (bool)element.GetValue(IsFocusedProperty);
        }

        /// <summary>
        /// Whether or not to hide the default blue box cursor on this Element?
        /// </summary>
        public static readonly DependencyProperty HideCursorProperty = DependencyProperty.RegisterAttached(
		    "HideCursor",
            typeof(bool),
            typeof(BlockNavigator),
            new PropertyMetadata(default(bool))
        );

        public static void SetHideCursor(FrameworkElement element, bool value) {
            element.SetValue(HideCursorProperty, value);
        }

        public static bool GetHideCursor(FrameworkElement element) {
            return (bool)element.GetValue(HideCursorProperty);
        }

        /// <summary>
        /// This is used to determine set the start focus of your UI
        /// </summary>
        public static readonly DependencyProperty InitialFocusProperty = DependencyProperty.RegisterAttached(
		    "InitialFocus",
            typeof(bool),
            typeof(BlockNavigator),
            new PropertyMetadata(default(bool))
        );

        public static bool GetInitialFocus(FrameworkElement element) {
            return (bool)element.GetValue(InitialFocusProperty);
        }

        public static void SetInitialFocus(FrameworkElement element, bool value) {
            element.SetValue(InitialFocusProperty, value);
        }

        /// <summary>
        /// Work in progress: This will enable you to design nested blocks in a container that shouldn't be navigatable without first activating it.
        /// This will also stop all blocks from the parent from being used.
        /// Confirm button will open the nested group. And Exit button will exit the group.
        /// </summary>
        public static readonly DependencyProperty IsNestedNavigationProperty = DependencyProperty.RegisterAttached(
            "IsNestedNavigation",
            typeof(bool),
            typeof(BlockNavigator),
            new FrameworkPropertyMetadata((a,b)=> {
                Utils.RegisterNestedNavigationBlock((FrameworkElement)a);
            })
        );

        public static bool GetIsNestedNavigation(FrameworkElement element) {
            return (bool)element.GetValue(IsNestedNavigationProperty);
        }

        public static void SetIsNestedNavigation(FrameworkElement element, bool value) {
            element.SetValue(IsNestedNavigationProperty, value);
        }

        /// <summary>
        /// Pass "Confirm button" to "Click" and "MouseUp". Useful for adding gamepad controls to an existing WPF application
        /// NOTE: Work in progress. Not actually working.
        /// </summary>
	    public static readonly DependencyProperty SimulateMouseProperty = DependencyProperty.RegisterAttached(
            "SimulateMouse",
            typeof(bool),
            typeof(BlockNavigator),
            new PropertyMetadata((a, b)=> Utils.RegisterNavigationBlock((FrameworkElement)a))
        );

        public static bool GetSimulateMouse(FrameworkElement element) {
            return (bool)element.GetValue(SimulateMouseProperty);
        }

        public static void SetSimulateMouse(FrameworkElement element, bool value) {
            element.SetValue(SimulateMouseProperty, value);
        }

        /// <summary>
        /// Contains the instance of our navigation system. Not singleton. It is per NestedNavigation. It's also a private member, no touchie
        /// </summary>
        internal static readonly DependencyProperty BlockNavigatorProperty = DependencyProperty.RegisterAttached(
            "BlockNavigator",
            typeof(BlockNavigatorInternal),
            typeof(BlockNavigator),
            new FrameworkPropertyMetadata(default(BlockNavigatorInternal))
        );

        internal static BlockNavigatorInternal GetBlockNavigator(FrameworkElement element) {
            return (BlockNavigatorInternal)element.GetValue(BlockNavigatorProperty);
        }

        internal static void SetBlockNavigator(FrameworkElement element, BlockNavigatorInternal value) {
            element.SetValue(BlockNavigatorProperty, value);
        }
    }
}
