using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace PadOS.Input {
	public class WPFGamepad{
		public static readonly GamePadInput XInput = new GamePadInput();
		private static readonly Dictionary<System.Windows.UIElement, WPFGamepad> Wrappers = new Dictionary<System.Windows.UIElement, WPFGamepad>();

		private static System.Windows.UIElement _windowControl;

		public class GamePadEventArgs : EventArgs {
			public XInputDotNetPure.PlayerIndex Player { get; set; }
			public XInputDotNetPure.GamePadState GamePadState { get; set; }
		}

		public class GamePadEventArgs<T> : GamePadEventArgs {
			public T Value { get; set; }
		}

		public delegate void GamepadEvent(object sender, GamePadEventArgs args);
		public delegate void GamepadEvent<T>(object sender, GamePadEventArgs<T> args);

		public event GamepadEvent GamePadChange;
		public event GamepadEvent ButtonADown;
		public event GamepadEvent ButtonAUp;
		public event GamepadEvent ButtonBDown;
		public event GamepadEvent ButtonBUp;
		public event GamepadEvent ButtonXDown;
		public event GamepadEvent ButtonXUp;
		public event GamepadEvent ButtonYDown;
		public event GamepadEvent ButtonYUp;
		public event GamepadEvent ButtonBackDown;
		public event GamepadEvent ButtonBackUp;
		public event GamepadEvent ButtonGuideDown;
		public event GamepadEvent ButtonGuideUp;
		public event GamepadEvent ButtonLeftShoulderDown;
		public event GamepadEvent ButtonLeftShoulderUp;
		public event GamepadEvent ButtonLeftStickDown;
		public event GamepadEvent ButtonLeftStickUp;
		public event GamepadEvent ButtonRightShoulderDown;
		public event GamepadEvent ButtonRightShoulderUp;
		public event GamepadEvent ButtonRightStickDown;
		public event GamepadEvent ButtonRightStickUp;
		public event GamepadEvent ButtonStartDown;
		public event GamepadEvent ButtonStartUp;
		public event GamepadEvent DPadLeftDown;
		public event GamepadEvent DPadLeftUp;
		public event GamepadEvent DPadRightDown;
		public event GamepadEvent DPadRightUp;
		public event GamepadEvent DPadUpDown;
		public event GamepadEvent DPadUpUp;
		public event GamepadEvent DPadDownDown;
		public event GamepadEvent DPadDownUp;
		public event GamepadEvent<Vector2> ThumbLeftChange;
		public event GamepadEvent<Vector2> ThumbRightChange;
		public event GamepadEvent<float> TriggerLeftChange;
		public event GamepadEvent<float> TriggerRightChange;

		static WPFGamepad(){
			XInput.ButtonADown				+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonADown);
			XInput.ButtonAUp				+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonAUp);
			XInput.ButtonBDown				+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonBDown);
			XInput.ButtonBUp				+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonBUp);
			XInput.ButtonXDown				+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonXDown);
			XInput.ButtonXUp				+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonXUp);
			XInput.ButtonYDown				+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonYDown);
			XInput.ButtonYUp				+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonYUp);
			XInput.ButtonBackDown			+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonBackDown);
			XInput.ButtonBackUp				+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonBackUp);
			XInput.ButtonGuideDown			+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonGuideDown);
			XInput.ButtonGuideUp			+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonGuideUp);
			XInput.ButtonLeftShoulderDown	+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonLeftShoulderDown);
			XInput.ButtonLeftShoulderUp		+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonLeftShoulderUp);
			XInput.ButtonLeftStickDown		+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonLeftStickDown);
			XInput.ButtonLeftStickUp		+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonLeftStickUp);
			XInput.ButtonRightShoulderDown	+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonRightShoulderDown);
			XInput.ButtonRightShoulderUp	+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonRightShoulderUp);
			XInput.ButtonRightStickDown		+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonRightStickDown);
			XInput.ButtonRightStickUp		+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonRightStickUp);
			XInput.ButtonStartDown			+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonStartDown);
			XInput.ButtonStartUp			+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ButtonStartUp);
			XInput.DPadLeftDown				+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].DPadLeftDown);
			XInput.DPadLeftUp				+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].DPadLeftUp);
			XInput.DPadRightDown			+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].DPadRightDown);
			XInput.DPadRightUp				+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].DPadRightUp);
			XInput.DPadUpDown				+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].DPadUpDown);
			XInput.DPadUpUp					+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].DPadUpUp);
			XInput.DPadDownDown				+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].DPadDownDown);
			XInput.DPadDownUp				+= (player, state)=>Dispatch(player, state, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].DPadDownUp);
			XInput.ThumbLeftChange			+= (player, state, value)=>Dispatch(player, state, value, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ThumbLeftChange);
			XInput.ThumbRightChange			+= (player, state, value)=>Dispatch(player, state, value, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].ThumbRightChange);
			XInput.TriggerLeftChange		+= (player, state, value)=>Dispatch(player, state, value, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].TriggerLeftChange);
			XInput.TriggerRightChange		+= (player, state, value)=>Dispatch(player, state, value, _windowControl!=null?Wrappers[_windowControl]:null, ()=>Wrappers[_windowControl].TriggerRightChange);
		}

		private static void Dispatch(XInputDotNetPure.PlayerIndex player, XInputDotNetPure.GamePadState state, WPFGamepad gamePadHandler, Func<GamepadEvent> evt){
			if (gamePadHandler == null) return;
			if (_windowControl == null) return;
			if (Wrappers.ContainsKey(_windowControl) == false) return;

			if (_windowControl == null) return;
			_windowControl.Dispatcher.BeginInvoke(new Action(() =>{
				var focussedElm = System.Windows.Input.FocusManager.GetFocusedElement(_windowControl);

				var handler = evt();
				if (handler != null) handler(focussedElm, new GamePadEventArgs{
					Player = player,
					GamePadState = state
				});

				if (gamePadHandler.GamePadChange != null) gamePadHandler.GamePadChange(focussedElm, new GamePadEventArgs{
					Player = player,
					GamePadState = state
				});
			}));
		}

		private static void Dispatch<T>(XInputDotNetPure.PlayerIndex player, XInputDotNetPure.GamePadState state, T value, WPFGamepad gamePadHandler, Func<GamepadEvent<T>> evt) {
			if (gamePadHandler == null) return;
			if (_windowControl == null) return;
			if (Wrappers.ContainsKey(_windowControl) == false) return;

			if (_windowControl == null) return;
			_windowControl.Dispatcher.BeginInvoke(new Action(() => {
				var focussedElm = System.Windows.Input.FocusManager.GetFocusedElement(_windowControl);

				var handler = evt();
				if (handler != null) handler(focussedElm, new GamePadEventArgs<T>{
					Player = player,
					Value = value,
					GamePadState = state
				});

				if (gamePadHandler.GamePadChange != null) gamePadHandler.GamePadChange(focussedElm, new GamePadEventArgs {
					Player = player,
					GamePadState = state
				});
			}));
		}

		public static WPFGamepad Register(System.Windows.UIElement control){
			WPFGamepad wrapper;
			if (Wrappers.ContainsKey(control))
				wrapper = Wrappers[control];
			else
				wrapper = Wrappers[control] = new WPFGamepad();

			control.GotKeyboardFocus -= OnControlOnGotFocus;
			control.GotKeyboardFocus += OnControlOnGotFocus;
			if (control.IsFocused)
				_windowControl = control;

			return wrapper;
		}

		private static void OnControlOnGotFocus(object sender, System.Windows.RoutedEventArgs args){
			_windowControl = (Control)sender;
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
