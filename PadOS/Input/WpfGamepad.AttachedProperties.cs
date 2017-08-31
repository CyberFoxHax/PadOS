using System;
using System.Windows;

namespace PadOS.Input {
	public partial class WpfGamePad {
		private static readonly DependencyProperty InstanceProperty = DependencyProperty.RegisterAttached(
			"Instance", typeof(WpfGamePad), typeof(WpfGamePad), new FrameworkPropertyMetadata(default(WpfGamePad)));
		public static WpfGamePad GetInstance(UIElement element) => (WpfGamePad)element.GetValue(InstanceProperty);
		private static void SetInstance(UIElement element, WpfGamePad value = null) => element.SetValue(InstanceProperty, value ?? new WpfGamePad(element));

		public static readonly DependencyProperty IsFocusableProperty = DependencyProperty.RegisterAttached(
			"IsFocusable", typeof(bool), typeof(WpfGamePad), new FrameworkPropertyMetadata(PropertyChangedCallback));
		public static bool GetIsFocusable(UIElement element) => throw new NotImplementedException();
		public static void SetIsFocusable(UIElement element, bool value) => element.SetValue(IsFocusableProperty, value);
		private static void PropertyChangedCallback(DependencyObject dep, DependencyPropertyChangedEventArgs args) {
			SetInstance((UIElement)dep);
		}
	}
}
