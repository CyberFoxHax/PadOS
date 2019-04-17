using System;
using System.Windows;

namespace PadOS.Input.WpfGamePad {
	public partial class WpfGamePad {
		private static readonly DependencyProperty InstanceProperty = DependencyProperty.RegisterAttached(
			"Instance", typeof(WpfGamePad), typeof(WpfGamePad), new FrameworkPropertyMetadata(default(WpfGamePad)));
		public static WpfGamePad GetInstance(UIElement element) => (WpfGamePad)element.GetValue(InstanceProperty);
		private static void SetInstance(UIElement element, WpfGamePad value) => element.SetValue(InstanceProperty, value);

        public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached(
            "IsFocused", typeof(bool), typeof(WpfGamePad), new FrameworkPropertyMetadata(IsFocusedPropertyChangedCallback));
        public static bool GetIsFocused(UIElement element) => (bool)element.GetValue(IsFocusedProperty);
        public static void SetIsFocused(UIElement element, bool value) => element.SetValue(IsFocusedProperty, value);
        private static void IsFocusedPropertyChangedCallback(DependencyObject dep, DependencyPropertyChangedEventArgs args) {
            var instance = GetInstance((UIElement)dep);
            instance.FocusChanged((bool)args.NewValue);
        }

        public static readonly DependencyProperty RegisteredProperty = DependencyProperty.RegisterAttached(
			"Registered", typeof(bool), typeof(WpfGamePad), new FrameworkPropertyMetadata(RegisteredPropertyChangedCallback));
		public static bool GetRegistered(UIElement element) => throw new NotImplementedException();
		public static void SetRegistered(UIElement element, bool value) => element.SetValue(RegisteredProperty, value);
		private static void RegisteredPropertyChangedCallback(DependencyObject dep, DependencyPropertyChangedEventArgs args) {
            if ((bool)args.NewValue == true)
                SetInstance((UIElement)dep, new WpfGamePad((UIElement)dep));
            else
                dep.ClearValue(InstanceProperty);
        }
    }
}
