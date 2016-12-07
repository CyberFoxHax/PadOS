using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace PadOS.Input {
	public static class WPFGamepad{
		private static readonly GamePadInput XInput = new GamePadInput();
		private static readonly Dictionary<System.Windows.UIElement, EventWrapper> Wrappers = new Dictionary<System.Windows.UIElement, EventWrapper>();
		private static System.Windows.UIElement _windowControl;

		public static System.Windows.UIElement WindowControl {
			get { return _windowControl; }
		}

		public delegate void GamepadEventArgs(object sender, XInputDotNetPure.GamePadState state);

		public class EventWrapper{
			public GamepadEventArgs GamePadOnChange;
		}

		static WPFGamepad(){
			XInput.GamePadChange += XInputOnGamePadChange;
		}

		private static void XInputOnGamePadChange(XInputDotNetPure.GamePadState input){
			if (_windowControl == null) return;
			if (Wrappers.ContainsKey(_windowControl) == false) return;
			if (Wrappers[_windowControl].GamePadOnChange == null) return;

			_windowControl.Dispatcher.BeginInvoke(new Action(() => {
				if (_windowControl == null) return;
				var focussedElm = System.Windows.Input.FocusManager.GetFocusedElement(_windowControl);
				Wrappers[_windowControl].GamePadOnChange(focussedElm, input);
			}));
		}

		public static EventWrapper Register(System.Windows.UIElement control){
			EventWrapper wrapper;
			if (Wrappers.ContainsKey(control))
				wrapper = Wrappers[control];
			else
				wrapper = Wrappers[control] = new EventWrapper();

			control.GotKeyboardFocus -= OnControlOnGotFocus;
			control.GotKeyboardFocus += OnControlOnGotFocus;
			if (control.IsFocused)
				_windowControl = control;

			return wrapper;
		}

		private static void OnControlOnGotFocus(object sender, System.Windows.RoutedEventArgs args){
			_windowControl = (Control) sender;
			_windowControl.LostKeyboardFocus += OnControlOnLostFocus;
			_windowControl.GotKeyboardFocus -= OnControlOnGotFocus;
		}

		private static void OnControlOnLostFocus(object sender, System.Windows.RoutedEventArgs args){
			if (_windowControl != sender) return;
			_windowControl.LostFocus -= OnControlOnLostFocus;
			_windowControl.GotKeyboardFocus += OnControlOnGotFocus;
			_windowControl = null;
		}
	}
}
