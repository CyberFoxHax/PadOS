using System;
using System.Collections.Generic;
using System.Linq;

namespace PadOS.Views.Settings {
	public class VerticalGamePadNavagtion{
		public static VerticalGamePadNavagtion Register(Input.IGamePadFocusable window, IEnumerable<INavigatable> itemList, System.Windows.Threading.Dispatcher dispatcher) {
			return new VerticalGamePadNavagtion(window, itemList, dispatcher);
		}

		public VerticalGamePadNavagtion(Input.IGamePadFocusable window, IEnumerable<INavigatable> itemList, System.Windows.Threading.Dispatcher dispatcher){
			_dispatcher = dispatcher;
			_buttonsList = itemList.ToArray();

			// https://msdn.microsoft.com/en-us/library/system.windows.forms.systeminformation.keyboarddelay%28v=vs.110%29.aspx
			_holdDelay.Interval = (System.Windows.Forms.SystemInformation.KeyboardDelay + 1) * 250;
			_repeatInterval.Interval = System.Windows.Forms.SystemInformation.KeyboardSpeed;
			_initialRepeatInterval = _repeatInterval.Interval;

			_gamepadEvents = Input.WPFGamepad.Register(window);
			_gamepadEvents.DPadDownDown += OnDPadDownDown;
			_gamepadEvents.DPadUpDown += OnDPadUpDown;
			_gamepadEvents.DPadDownUp += OnDPadUp;
			_gamepadEvents.DPadUpUp += OnDPadUp;
			_gamepadEvents.ButtonADown += GamepadEventsOnButtonADown;

			_gamepadEvents.ThumbLeftChange += OnThumbLeftChangeInitial;

			var buttons = _buttonsList;
			foreach (var button in buttons)
				button.IsActive = false;
			_activeItem = buttons.First();
			_activeItem.IsActive = true;

			_holdDelay.Elapsed += HoldDelayOnElapsed;
			_repeatInterval.Elapsed += RepeatIntervalOnElapsed;
		}

		public void Dispose(){
			_gamepadEvents.DPadDownDown -= OnDPadDownDown;
			_gamepadEvents.DPadUpDown -= OnDPadUpDown;
			_gamepadEvents.DPadDownUp -= OnDPadUp;
			_gamepadEvents.DPadUpUp -= OnDPadUp;
			_gamepadEvents.ThumbLeftChange -= OnThumbLeftChangeInitial;
			_gamepadEvents.ButtonADown -= GamepadEventsOnButtonADown;
			_gamepadEvents.Dispose();
			_holdDelay.Elapsed -= HoldDelayOnElapsed;
			_repeatInterval.Elapsed -= RepeatIntervalOnElapsed;
			_holdDelay.Dispose();
			_repeatInterval.Dispose();
			_repeatFunction = null;
		}

		private readonly System.Windows.Threading.Dispatcher _dispatcher;
		private INavigatable[] _buttonsList;
		private INavigatable _activeItem;
		private readonly System.Timers.Timer _holdDelay = new System.Timers.Timer(200) { AutoReset = false };
		private readonly System.Timers.Timer _repeatInterval = new System.Timers.Timer(66) { AutoReset = true };
		private Action _repeatFunction;
		private const double MinRepeatInterval = 33;
		private const double MaxRepeatInterval = 400;
		private readonly Input.WPFGamepad _gamepadEvents;
		private readonly double _initialRepeatInterval;

		public void UpdateItems(IEnumerable<INavigatable> buttonsList){
			_buttonsList = buttonsList.ToArray();
		}

		private void GamepadEventsOnButtonADown(object sender, Input.WPFGamepad.GamePadEventArgs args) {
			_activeItem.OnClick();
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
			var length = (Math.Abs(args.Value.Y) - threshhold) / (1 - threshhold); // todo deadzone not working

			const double min = MinRepeatInterval;
			const double max = MaxRepeatInterval - min;

			Input.WPFGamepad.GamepadEvent handlerDown;
			Input.WPFGamepad.GamepadEvent handlerUp = OnDPadUp;

			if (args.Value.Y < 0)		handlerDown = OnDPadDownDown;
			else if (args.Value.Y > 0)	handlerDown = OnDPadUpDown;
			else
				return;

			if (length > threshhold && _repeatFunction == null)
				handlerDown(null, null);
			else if (length < threshhold && _repeatFunction != null)
			    handlerUp(null, null);
			else
			    _repeatInterval.Interval = max - max*length + min;
		}

		private void HoldDelayOnElapsed(object sender, System.Timers.ElapsedEventArgs args) {
			_repeatInterval.Start();
		}

		private void RepeatIntervalOnElapsed(object sender, System.Timers.ElapsedEventArgs args) {
			if (_repeatFunction != null)
				_repeatFunction();
		}

		private void OnDPadUpDown(object sender, Input.WPFGamepad.GamePadEventArgs args) {
			_repeatInterval.Interval = _initialRepeatInterval;
			MoveUp();
			_repeatFunction = MoveUp;
			_holdDelay.Start();
		}

		private void OnDPadDownDown(object sender, Input.WPFGamepad.GamePadEventArgs args) {
			_repeatInterval.Interval = _initialRepeatInterval;
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
			var oldItem = _activeItem;
			var newItem = buttons[index <= 0 ? buttons.Length - 1 : index - 1];
			_activeItem = newItem;
			_dispatcher.BeginInvoke(new Action(() => {
				oldItem.IsActive = false;
				newItem.IsActive = true;
			}));
		}

		private void MoveDown() {
			var buttons = _buttonsList.ToArray();
			var index = Array.IndexOf(buttons, _activeItem);
			var oldItem = _activeItem;
			var newItem = buttons[(index + 1) % buttons.Length];
			_activeItem = newItem;
			_dispatcher.BeginInvoke(new Action(() => {
				oldItem.IsActive = false;
				newItem.IsActive = true;
			}));
		}
	}
}
