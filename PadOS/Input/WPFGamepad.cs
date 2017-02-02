using System;
using System.Collections.Generic;
using System.Linq;

namespace PadOS.Input {
	public class WPFGamepad{
		public static readonly GamePadInput XInput = new GamePadInput();
		private static readonly Dictionary<IGamePadFocusable, WPFGamepad> Wrappers = new Dictionary<IGamePadFocusable, WPFGamepad>();

		private static IGamePadFocusable _focusedControl;

		public class GamePadEventArgs : EventArgs {
			public XInputDotNetPure.PlayerIndex Player { get; set; }
			public XInputDotNetPure.GamePadState GamePadState { get; set; }
		}

		public class GamePadEventArgs<T> : GamePadEventArgs {
			public T Value { get; set; }
		}

		public void Dispose(){
			Wrappers.Remove(Wrappers.First(p=>p.Value==this).Key);
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
			XInput.ButtonADown				+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonADown);
			XInput.ButtonAUp				+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonAUp);
			XInput.ButtonBDown				+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonBDown);
			XInput.ButtonBUp				+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonBUp);
			XInput.ButtonXDown				+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonXDown);
			XInput.ButtonXUp				+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonXUp);
			XInput.ButtonYDown				+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonYDown);
			XInput.ButtonYUp				+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonYUp);
			XInput.ButtonBackDown			+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonBackDown);
			XInput.ButtonBackUp				+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonBackUp);
			XInput.ButtonGuideDown			+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonGuideDown);
			XInput.ButtonGuideUp			+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonGuideUp);
			XInput.ButtonLeftShoulderDown	+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonLeftShoulderDown);
			XInput.ButtonLeftShoulderUp		+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonLeftShoulderUp);
			XInput.ButtonLeftStickDown		+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonLeftStickDown);
			XInput.ButtonLeftStickUp		+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonLeftStickUp);
			XInput.ButtonRightShoulderDown	+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonRightShoulderDown);
			XInput.ButtonRightShoulderUp	+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonRightShoulderUp);
			XInput.ButtonRightStickDown		+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonRightStickDown);
			XInput.ButtonRightStickUp		+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonRightStickUp);
			XInput.ButtonStartDown			+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonStartDown);
			XInput.ButtonStartUp			+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ButtonStartUp);
			XInput.DPadLeftDown				+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].DPadLeftDown);
			XInput.DPadLeftUp				+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].DPadLeftUp);
			XInput.DPadRightDown			+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].DPadRightDown);
			XInput.DPadRightUp				+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].DPadRightUp);
			XInput.DPadUpDown				+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].DPadUpDown);
			XInput.DPadUpUp					+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].DPadUpUp);
			XInput.DPadDownDown				+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].DPadDownDown);
			XInput.DPadDownUp				+= (player, state)=>Dispatch(player, state, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].DPadDownUp);
			XInput.ThumbLeftChange			+= (player, state, value)=>Dispatch(player, state, value, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ThumbLeftChange);
			XInput.ThumbRightChange			+= (player, state, value)=>Dispatch(player, state, value, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].ThumbRightChange);
			XInput.TriggerLeftChange		+= (player, state, value)=>Dispatch(player, state, value, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].TriggerLeftChange);
			XInput.TriggerRightChange		+= (player, state, value)=>Dispatch(player, state, value, _focusedControl!=null?Wrappers[_focusedControl]:null, ()=>Wrappers[_focusedControl].TriggerRightChange);
		}

		private static void Dispatch(XInputDotNetPure.PlayerIndex player, XInputDotNetPure.GamePadState state, WPFGamepad gamePadHandler, Func<GamepadEvent> evt){
			if (gamePadHandler == null) return;
			if (_focusedControl == null) return;
			if (Wrappers.ContainsKey(_focusedControl) == false) return;

			if (_focusedControl == null) return;
			var focussedElm = _focusedControl;

			var handler = evt();
			if (handler != null) handler(focussedElm, new GamePadEventArgs{
				Player = player,
				GamePadState = state
			});

			if (gamePadHandler.GamePadChange != null) gamePadHandler.GamePadChange(focussedElm, new GamePadEventArgs{
				Player = player,
				GamePadState = state
			});
		}

		private static void Dispatch<T>(XInputDotNetPure.PlayerIndex player, XInputDotNetPure.GamePadState state, T value, WPFGamepad gamePadHandler, Func<GamepadEvent<T>> evt) {
			if (gamePadHandler == null) return;
			if (_focusedControl == null) return;
			if (Wrappers.ContainsKey(_focusedControl) == false) return;

			var focussedElm = _focusedControl;

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
		}

		public static WPFGamepad Register(IGamePadFocusable control){
			WPFGamepad wrapper;
			if (Wrappers.ContainsKey(control))
				wrapper = Wrappers[control];
			else
				wrapper = Wrappers[control] = new WPFGamepad();

			if (control.IsGamePadFocused)
				_focusedControl = control;

			return wrapper;
		}

		public static void Focus(IGamePadFocusable ctrl){
			if (_focusedControl != null)
				_focusedControl.IsGamePadFocused = false;

			if (ctrl == null) return;
			_focusedControl = ctrl;
			_focusedControl.IsGamePadFocused = true;
		}
	}
}
