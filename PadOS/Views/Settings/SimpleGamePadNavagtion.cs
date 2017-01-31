using System;
using System.Collections.Generic;
using System.Linq;

namespace PadOS.Views.Settings {
	public class SimpleGamePadNavagtion{
		public static SimpleGamePadNavagtion Register(System.Windows.UIElement window, IEnumerable<INavigatable> itemList) {
			return new SimpleGamePadNavagtion(window, itemList);
		}

		public SimpleGamePadNavagtion(System.Windows.UIElement window, IEnumerable<INavigatable> itemList){
			_window = window;
			_buttonsList = itemList.ToArray();

			// https://msdn.microsoft.com/en-us/library/system.windows.forms.systeminformation.keyboarddelay%28v=vs.110%29.aspx
			_holdDelay.Interval = (System.Windows.Forms.SystemInformation.KeyboardDelay + 1) * 250;
			_repeatInterval.Interval = System.Windows.Forms.SystemInformation.KeyboardSpeed;

			_gamepadEvents = Input.WPFGamepad.Register(window);
			_gamepadEvents.DPadDownDown += OnDPadDownDown;
			_gamepadEvents.DPadUpDown += OnDPadUpDown;
			_gamepadEvents.DPadDownUp += OnDPadUp;
			_gamepadEvents.DPadUpUp += OnDPadUp;

			_gamepadEvents.ThumbLeftChange += OnThumbLeftChangeInitial;

			var buttons = _buttonsList;
			foreach (var button in buttons)
				button.IsActive = false;
			_activeItem = buttons.First();
			_activeItem.IsActive = true;

			_holdDelay.Elapsed += HoldDelayOnElapsed;
			_repeatInterval.Elapsed += RepeatIntervalOnElapsed;
		}

		private readonly System.Windows.UIElement _window;
		private INavigatable[] _buttonsList;
		private INavigatable _activeItem;
		private readonly System.Timers.Timer _holdDelay = new System.Timers.Timer(200) { AutoReset = false };
		private readonly System.Timers.Timer _repeatInterval = new System.Timers.Timer(66) { AutoReset = true };
		private Action _repeatFunction;
		private readonly double _minRepeatInterval = 33;
		private readonly double _maxRepeatInterval = 400;
		private readonly Input.WPFGamepad _gamepadEvents;

		public void UpdateItems(IEnumerable<INavigatable> buttonsList){
			_buttonsList = buttonsList.ToArray();
		}

		private void OnThumbLeftChangeInitial(object sender, Input.WPFGamepad.GamePadEventArgs<Vector2> args) {
			const float threshhold = 0.3f;
			var length = Math.Abs(args.Value.Y);
			if (length > threshhold) return;

			_gamepadEvents.ThumbLeftChange -= OnThumbLeftChangeInitial;
			_gamepadEvents.ThumbLeftChange += OnThumbLeftChange;
			OnThumbLeftChange(sender, args);
		}

		private void OnThumbLeftChange(object sender, Input.WPFGamepad.GamePadEventArgs<Vector2> args){
			const float threshhold = 0.3f;
			var length = (Math.Abs(args.Value.Y)-threshhold)/(1-threshhold);

			var min = _minRepeatInterval;
			var max = _maxRepeatInterval - min;

			Input.WPFGamepad.GamepadEvent handlerDown;
			Input.WPFGamepad.GamepadEvent handlerUp = OnDPadUp;

			if (args.Value.Y < 0)		handlerDown = OnDPadDownDown;
			else if (args.Value.Y > 0)	handlerDown = OnDPadUpDown;
			else
				return;

			if (length > 0 && _repeatFunction == null)
				handlerDown(null, null);
			else if (length < 0 && _repeatFunction != null)
				handlerUp(null, null);
			else
				_repeatInterval.Interval = max - max * length + min;
		}

		private void HoldDelayOnElapsed(object sender, System.Timers.ElapsedEventArgs args) {
			_repeatInterval.Start();
		}

		private void RepeatIntervalOnElapsed(object sender, System.Timers.ElapsedEventArgs args) {
			if (_repeatFunction != null)
				_window.Dispatcher.BeginInvoke(_repeatFunction);
		}

		private void OnDPadUpDown(object sender, Input.WPFGamepad.GamePadEventArgs args) {
			MoveUp();
			_repeatFunction = MoveUp;
			_holdDelay.Start();
		}

		private void OnDPadDownDown(object sender, Input.WPFGamepad.GamePadEventArgs args) {
			MoveDown();
			_repeatFunction = MoveDown;
			_holdDelay.Start();
		}

		private void OnDPadUp(object sender, Input.WPFGamepad.GamePadEventArgs args) {
			_repeatFunction = null;
			_holdDelay.Stop();
			_repeatInterval.Stop();
		}

		private void MoveUp() {
			var buttons = _buttonsList.ToArray();
			var index = Array.IndexOf(buttons, _activeItem);
			_activeItem.IsActive = false;
			_activeItem = buttons[index <= 0 ? buttons.Length - 1 : index - 1];
			_activeItem.IsActive = true;
		}

		private void MoveDown() {
			var buttons = _buttonsList.ToArray();
			var index = Array.IndexOf(buttons, _activeItem);
			_activeItem.IsActive = false;
			_activeItem = buttons[(index + 1) % buttons.Length];
			_activeItem.IsActive = true;
		}
	}
}
