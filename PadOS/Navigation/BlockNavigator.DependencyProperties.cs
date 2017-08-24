using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PadOS.Navigation
{
    public partial class BlockNavigator{

	    public delegate void NavigationEvent(object sender, EventArgs args);

		public static readonly RoutedEvent CursorExitEvent = EventManager.RegisterRoutedEvent(
		    "CursorExitEvent", RoutingStrategy.Bubble, typeof(NavigationEvent), typeof(BlockNavigator));
	    public static void AddCursorExitHandler(DependencyObject d, NavigationEvent handler) => (d as UIElement)?.AddHandler(CursorExitEvent, handler);
	    public static void RemoveCursorExitHandler(DependencyObject d, NavigationEvent handler) => (d as UIElement)?.RemoveHandler(CursorExitEvent, handler);

	    public static readonly RoutedEvent CursorEnterEvent = EventManager.RegisterRoutedEvent(
		    "CursorEnter", RoutingStrategy.Bubble, typeof(NavigationEvent), typeof(BlockNavigator));
	    public static void AddCursorEnterHandler(DependencyObject d, NavigationEvent handler) => (d as UIElement)?.AddHandler(CursorEnterEvent, handler);
	    public static void RemoveCursorEnterHandler(DependencyObject d, NavigationEvent handler) => (d as UIElement)?.RemoveHandler(CursorEnterEvent, handler);

		public static readonly RoutedEvent ConfirmClickEvent = EventManager.RegisterRoutedEvent(
			"ConfirmClick", RoutingStrategy.Bubble, typeof(NavigationEvent), typeof(BlockNavigator));
		public static void AddConfirmClickHandler(DependencyObject d, NavigationEvent handler) => (d as UIElement)?.AddHandler(ConfirmClickEvent, handler);
		public static void RemoveConfirmClickHandler(DependencyObject d, NavigationEvent handler) => (d as UIElement)?.RemoveHandler(ConfirmClickEvent, handler);

	    public static readonly RoutedEvent CancelClickEvent = EventManager.RegisterRoutedEvent(
		    "CancelClick", RoutingStrategy.Bubble, typeof(NavigationEvent), typeof(BlockNavigator));
	    public static void AddCancelClickHandler(DependencyObject d, NavigationEvent handler) => (d as UIElement)?.AddHandler(CancelClickEvent, handler);
	    public static void RemoveCancelClickHandler(DependencyObject d, NavigationEvent handler) => (d as UIElement)?.RemoveHandler(CancelClickEvent, handler);

		/////////////////////////////////////////

		public static readonly DependencyProperty InitialFocusProperty = DependencyProperty.RegisterAttached(
		    "InitialFocus", typeof(bool), typeof(BlockNavigator), new PropertyMetadata(default(bool)));
	    public static bool GetInitialFocus(DependencyObject element) => (bool)element.GetValue(InitialFocusProperty);
	    public static void SetInitialFocus(DependencyObject element, bool value) => element.SetValue(InitialFocusProperty, value);

	    private static readonly DependencyProperty BlockNavigatorProperty = DependencyProperty.RegisterAttached(
		    "BlockNavigator", typeof(BlockNavigator), typeof(BlockNavigator), new FrameworkPropertyMetadata(default(BlockNavigator)));
	    private static BlockNavigator GetBlockNavigator(UIElement element) => (BlockNavigator)element.GetValue(BlockNavigatorProperty);
	    private static void SetBlockNavigator(UIElement element, BlockNavigator value = null) => element.SetValue(BlockNavigatorProperty, value ?? new BlockNavigator(element));

	    public static readonly DependencyProperty IsBlockOwnerProperty = DependencyProperty.RegisterAttached(
		    "IsBlockOwner", typeof(bool), typeof(BlockNavigator), new FrameworkPropertyMetadata(PropertyChangedCallback));
	    public static bool GetIsBlockOwner(UIElement element) => throw new NotImplementedException();
	    public static void SetIsBlockOwner(UIElement element, bool value) => element.SetValue(IsBlockOwnerProperty, value);
	    private static void PropertyChangedCallback(DependencyObject dep, DependencyPropertyChangedEventArgs args) {
		    SetBlockNavigator((UIElement)dep);
	    }

	    public static readonly DependencyProperty RegisteredProperty = DependencyProperty.RegisterAttached(
		    "Registered", typeof(bool), typeof(BlockNavigator), new FrameworkPropertyMetadata(RegisteredChanged));
	    public static bool GetRegistered(UIElement element) => throw new NotImplementedException();
	    public static void SetRegistered(UIElement element, bool value) => element.SetValue(RegisteredProperty, value);
	}
}
