using System;
using System.Windows;

namespace PadOS.Navigation
{
    public partial class BlockNavigator{

	    public delegate void NavigationEvent(object sender, EventArgs args);

        private static void RegisterEvent(RoutedEvent evt, DependencyObject d, NavigationEvent handler) {
            (d as UIElement).AddHandler(evt, handler);
            RegisteredChanged(d);
        }

        private static void UnregisterEvent(RoutedEvent evt, DependencyObject d, NavigationEvent handler) {
            (d as UIElement).RemoveHandler(CursorEnterEvent, handler);
            RegisteredChanged(d);
        }

        /// <summary>
        /// When the cursor hovers the Element. "Sender" is the Element that contains the event
        /// </summary>
	    public static readonly RoutedEvent CursorEnterEvent = EventManager.RegisterRoutedEvent(
		    "CursorEnter",
            RoutingStrategy.Bubble,
            typeof(NavigationEvent),
            typeof(BlockNavigator)
        );

        public static void AddCursorEnterHandler(DependencyObject d, NavigationEvent handler) {
            RegisterEvent(CursorEnterEvent, d, handler);;
        }

        public static void RemoveCursorEnterHandler(DependencyObject d, NavigationEvent handler) {
            UnregisterEvent(CursorEnterEvent, d, handler);;
        }

        /// <summary>
        /// When the cursor's hover leaves the Element. "Sender" is the Element that contains the event
        /// </summary>
        public static readonly RoutedEvent CursorExitEvent = EventManager.RegisterRoutedEvent(
		    "CursorExit",
            RoutingStrategy.Bubble,
            typeof(NavigationEvent),
            typeof(BlockNavigator)
        );

        public static void AddCursorExitHandler(DependencyObject d, NavigationEvent handler) {
            RegisterEvent(CursorExitEvent, d, handler);;
        }

        public static void RemoveCursorExitHandler(DependencyObject d, NavigationEvent handler) {
            UnregisterEvent(CursorExitEvent, d, handler);;
        }

        /// <summary>
        /// Confirm button event. Mapped to XInput:A. "Sender" is the Element that contains the event
        /// </summary>
        public static readonly RoutedEvent ConfirmClickEvent = EventManager.RegisterRoutedEvent(
			"ConfirmClick",
            RoutingStrategy.Bubble,
            typeof(NavigationEvent),
            typeof(BlockNavigator)
        );

        public static void AddConfirmClickHandler(DependencyObject d, NavigationEvent handler) {
            RegisterEvent(ConfirmClickEvent, d, handler);;
        }

        public static void RemoveConfirmClickHandler(DependencyObject d, NavigationEvent handler) {
            UnregisterEvent(ConfirmClickEvent, d, handler);;
        }

        /// <summary>
        /// Cancel button event. Mapped to XInput:B. "Sender" is the Element that contains the event
        /// </summary>
        public static readonly RoutedEvent CancelClickEvent = EventManager.RegisterRoutedEvent(
		    "CancelClick",
            RoutingStrategy.Bubble,
            typeof(NavigationEvent),
            typeof(BlockNavigator)
        );

        public static void AddCancelClickHandler(DependencyObject d, NavigationEvent handler) {
            RegisterEvent(CancelClickEvent, d, handler);;
        }

        public static void RemoveCancelClickHandler(DependencyObject d, NavigationEvent handler) {
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

        private static void SetIsFocused(DependencyObject element, bool value) {
            element.SetValue(IsFocusedProperty, value);
        }

        public static bool GetIsFocused(DependencyObject element) {
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

        public static void SetHideCursor(DependencyObject element, bool value) {
            element.SetValue(HideCursorProperty, value);
        }

        public static bool GetHideCursor(DependencyObject element) {
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

        public static bool GetInitialFocus(DependencyObject element) {
            return (bool)element.GetValue(InitialFocusProperty);
        }

        public static void SetInitialFocus(DependencyObject element, bool value) {
            element.SetValue(InitialFocusProperty, value);
        }

        /// <summary>
        /// Work in progress: This will enable you to design nested blocks in a container that shouldn't be navigatable without first activating it.
        /// This will also stop all blocks from the parent from being used.
        /// Confirm button will open the nested group. And Exit button will exit the group.
        /// </summary>
        public static readonly DependencyProperty NestedNavigationProperty = DependencyProperty.RegisterAttached(
            "NestedNavigation",
            typeof(bool),
            typeof(BlockNavigator),
            new FrameworkPropertyMetadata((a,b)=> {
                SetBlockNavigator((UIElement)a);
                RegisteredChanged(a);
            })
        );

        public static bool GetNestedNavigation(UIElement element) {
            return (bool)element.GetValue(NestedNavigationProperty);
        }

        public static void SetNestedNavigation(UIElement element, bool value) {
            element.SetValue(NestedNavigationProperty, value);
        }

        /// <summary>
        /// Pass "Confirm button" to "Click" and "MouseUp". Useful for adding gamepad controls to an existing WPF application
        /// NOTE: Work in progress. Not actually working.
        /// </summary>
	    public static readonly DependencyProperty SimulateMouseProperty = DependencyProperty.RegisterAttached(
            "SimulateMouse",
            typeof(bool),
            typeof(BlockNavigator),
            new PropertyMetadata((a, b)=> RegisteredChanged(a))
        );

        public static bool GetSimulateMouse(UIElement element) {
            return (bool)element.GetValue(SimulateMouseProperty);
        }

        public static void SetSimulateMouse(UIElement element, bool value) {
            element.SetValue(SimulateMouseProperty, value);
        }

        /// <summary>
        /// Contains the instance of our navigation system. Not singleton. It is per NestedNavigation. It's also a private member, no touchie
        /// </summary>
        private static readonly DependencyProperty BlockNavigatorProperty = DependencyProperty.RegisterAttached(
            "BlockNavigator",
            typeof(BlockNavigator),
            typeof(BlockNavigator),
            new FrameworkPropertyMetadata(default(BlockNavigator))
        );

        private static BlockNavigator GetBlockNavigator(UIElement element) {
            return (BlockNavigator)element.GetValue(BlockNavigatorProperty);
        }

        private static void SetBlockNavigator(UIElement element, BlockNavigator value = null) {
            element.SetValue(BlockNavigatorProperty, value ?? new BlockNavigator(element));
        }
    }
}
