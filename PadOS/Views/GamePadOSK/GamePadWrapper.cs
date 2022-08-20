using System;
using System.Windows;
using PadOS.Input;
using PadOS.Input.WpfGamePad;

namespace PadOS.Views.GamePadOSK {
	public class GamePadWrapper {
		private readonly DependencyObject _owner;

		public event Action<Input.Vector2> BlockPosChanged;
		public event Action<Input.Vector2> CharPosChanged;

        public event Action<double> OnScale;
        public event Action<Vector2> OnMove;

        public event Action ChangeCaseDown;
		public event Action ChangeSymbolsDown;
		public event Action DeleteDown;
		public event Action SpaceDown;
		public event Action EnterDown;
		public event Action MoveLeftDown;
		public event Action MoveRightDown;

		public event Action ChangeCaseUp;
		public event Action ChangeSymbolsUp;
		public event Action DeleteUp;
		public event Action SpaceUp;
		public event Action EnterUp;
		public event Action MoveLeftUp;
		public event Action MoveRightUp;

        public event Action HideLegend;

        private bool _changeSymbolsDown;
		private bool _changeCaseIsDown;
		private const float TriggerThreshold = 0.3f;

		public GamePadWrapper(DependencyObject owner){
			_owner = owner;
            WpfGamePad.AddButtonRightStickDownHandler(owner, OnHideLegend);
            WpfGamePad.AddThumbLeftChangeHandler(owner, OnBlockPosChanged);
            WpfGamePad.AddThumbRightChangeHandler(owner, OnRightThumbChanged);
            WpfGamePad.AddTriggerLeftChangeHandler(owner, OnChangeCase);
			WpfGamePad.AddTriggerRightChangeHandler(owner, OnChangeSymbols);

			WpfGamePad.AddButtonADownHandler(owner, OnCharUp);
			WpfGamePad.AddButtonBDownHandler(owner, OnCharRight);
			WpfGamePad.AddButtonXDownHandler(owner, OnCharLeft);
			WpfGamePad.AddButtonYDownHandler(owner, OnCharDown);
			WpfGamePad.AddDPadLeftDownHandler(owner, OnMoveLeftDown);
			WpfGamePad.AddDPadRightDownHandler(owner, OnMoveRightDown);
			WpfGamePad.AddDPadDownDownHandler(owner, OnEnterDown);
			WpfGamePad.AddButtonStartDownHandler(owner, OnEnterDown);
			WpfGamePad.AddButtonLeftShoulderDownHandler(owner, OnDeleteDown);
			WpfGamePad.AddButtonRightShoulderDownHandler(owner, OnSpaceDown);
			
			WpfGamePad.AddDPadLeftUpHandler(owner, OnMoveLeftUp);
			WpfGamePad.AddDPadRightUpHandler(owner, OnMoveRightUp);
			WpfGamePad.AddDPadDownUpHandler(owner, OnEnterUp);
			WpfGamePad.AddButtonStartUpHandler(owner, OnEnterUp);
			WpfGamePad.AddButtonLeftShoulderUpHandler(owner, OnDeleteUp);
			WpfGamePad.AddButtonRightShoulderUpHandler(owner, OnSpaceUp);
            _timer.Elapsed += _timer_Elapsed;
		}

        private bool _blocking = false;
        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            if (_blocking)
                return;
            _blocking = true;
            _owner.Dispatcher.Invoke(()=> {
                if (_changeSymbolsDown) 
                    OnScale?.Invoke(_rightAnalogueValue.Y);
                else
                    OnMove?.Invoke(_rightAnalogueValue);
                _blocking = false;
            });
        }

        private Vector2 _rightAnalogueValue;
        private System.Timers.Timer _timer = new System.Timers.Timer { Interval = 30, AutoReset = true };

        private void OnRightThumbChanged(object sender, GamePadEventArgs<Vector2> args) {
            _rightAnalogueValue = args.Value;
            if (Math.Abs(args.Value.X) < 0.1 && Math.Abs(args.Value.Y) < 0.1)
                _timer.Stop();
            else {
                if(_timer.Enabled == false)
                    _timer.Start();
            }
        }

        private void OnChangeCase(object sender, GamePadEventArgs<float> args) {
			if (args.Value > TriggerThreshold && _changeCaseIsDown == false) {
				_changeCaseIsDown = true;
				ChangeCaseDown?.Invoke();
			}

			if (args.Value < TriggerThreshold && _changeCaseIsDown) {
				_changeCaseIsDown = false;
				ChangeCaseUp?.Invoke();
			}
		}

		private void OnChangeSymbols(object sender, GamePadEventArgs<float> args) {
			if (args.Value > TriggerThreshold && _changeSymbolsDown == false) {
				_changeSymbolsDown = true;
				ChangeSymbolsDown?.Invoke();
			}

			if (args.Value < TriggerThreshold && _changeSymbolsDown) {
				_changeSymbolsDown = false;
				ChangeSymbolsUp?.Invoke();
			}
		}

		private void OnSpaceDown(object sender, GamePadEventArgs args) => SpaceDown?.Invoke();
		private void OnDeleteDown(object sender, GamePadEventArgs args) => DeleteDown?.Invoke();
		private void OnEnterDown(object sender, GamePadEventArgs args) => EnterDown?.Invoke();
		private void OnMoveLeftDown(object sender, GamePadEventArgs args) => MoveLeftDown?.Invoke();
		private void OnMoveRightDown(object sender, GamePadEventArgs args) => MoveRightDown?.Invoke();
		private void OnEnterUp(object sender, GamePadEventArgs args) => EnterUp?.Invoke();
		private void OnSpaceUp(object sender, GamePadEventArgs args) => SpaceUp?.Invoke();
		private void OnDeleteUp(object sender, GamePadEventArgs args) => DeleteUp?.Invoke();
		private void OnMoveRightUp(object sender, GamePadEventArgs args) => MoveRightUp?.Invoke();
		private void OnMoveLeftUp(object sender, GamePadEventArgs args) => MoveLeftUp?.Invoke();
		private void OnCharDown(object sender, GamePadEventArgs args) => CharPosChanged?.Invoke(new Input.Vector2(0,-1));
		private void OnCharLeft(object sender, GamePadEventArgs args) => CharPosChanged?.Invoke(new Input.Vector2(-1,0));
		private void OnCharRight(object sender, GamePadEventArgs args) => CharPosChanged?.Invoke(new Input.Vector2(1,0));
		private void OnCharUp(object sender, GamePadEventArgs args) => CharPosChanged?.Invoke(new Input.Vector2(0,1));
		private void OnBlockPosChanged(object sender, GamePadEventArgs<Input.Vector2> args) => BlockPosChanged?.Invoke(args.Value);
        private void OnHideLegend(object sender, GamePadEventArgs args) => HideLegend?.Invoke();

        public void Dispose(){
            WpfGamePad.RemoveButtonRightStickDownHandler(_owner, OnHideLegend);
            WpfGamePad.RemoveThumbLeftChangeHandler(_owner, OnBlockPosChanged);
            WpfGamePad.RemoveThumbRightChangeHandler(_owner, OnRightThumbChanged);
            WpfGamePad.RemoveTriggerLeftChangeHandler(_owner, OnChangeCase);
			WpfGamePad.RemoveTriggerRightChangeHandler(_owner, OnChangeSymbols);

			WpfGamePad.RemoveButtonADownHandler(_owner, OnCharUp);
			WpfGamePad.RemoveButtonBDownHandler(_owner, OnCharRight);
			WpfGamePad.RemoveButtonXDownHandler(_owner, OnCharLeft);
			WpfGamePad.RemoveButtonYDownHandler(_owner, OnCharDown);
			WpfGamePad.RemoveDPadLeftDownHandler(_owner, OnMoveLeftDown);
			WpfGamePad.RemoveDPadRightDownHandler(_owner, OnMoveRightDown);
			WpfGamePad.RemoveDPadDownDownHandler(_owner, OnEnterDown);
			WpfGamePad.RemoveButtonStartDownHandler(_owner, OnEnterDown);
			WpfGamePad.RemoveButtonLeftShoulderDownHandler(_owner, OnDeleteDown);
			WpfGamePad.RemoveButtonRightShoulderDownHandler(_owner, OnSpaceDown);
			
			WpfGamePad.AddDPadLeftUpHandler(_owner, OnMoveLeftUp);
			WpfGamePad.AddDPadRightUpHandler(_owner, OnMoveRightUp);
			WpfGamePad.AddDPadDownUpHandler(_owner, OnEnterUp);
			WpfGamePad.AddButtonStartUpHandler(_owner, OnEnterUp);
			WpfGamePad.AddButtonLeftShoulderUpHandler(_owner, OnDeleteUp);
			WpfGamePad.AddButtonRightShoulderUpHandler(_owner, OnSpaceUp);
		}
	}
}
