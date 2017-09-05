using System;
using System.Windows;
using PadOS.Input;

namespace PadOS.Views.GamePadOSK {
	public class GamePadWrapper {
		private readonly DependencyObject _owner;

		public event Action<Vector2> BlockPosChanged;
		public event Action<Vector2> CharPosChanged;

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

		private bool _changeSymbolsDown;
		private bool _changeCaseIsDown;
		private const float TriggerThreshold = 0.3f;

		public GamePadWrapper(DependencyObject owner){
			_owner = owner;
			WpfGamePad.AddThumbLeftChangeHandler(owner, OnBlockPosChanged);
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
		private void OnCharDown(object sender, GamePadEventArgs args) => CharPosChanged?.Invoke(new Vector2(0,-1));
		private void OnCharLeft(object sender, GamePadEventArgs args) => CharPosChanged?.Invoke(new Vector2(-1,0));
		private void OnCharRight(object sender, GamePadEventArgs args) => CharPosChanged?.Invoke(new Vector2(1,0));
		private void OnCharUp(object sender, GamePadEventArgs args) => CharPosChanged?.Invoke(new Vector2(0,1));
		private void OnBlockPosChanged(object sender, GamePadEventArgs<Vector2> args) => BlockPosChanged?.Invoke(args.Value);

		public void Dispose(){
			WpfGamePad.RemoveThumbLeftChangeHandler(_owner, OnBlockPosChanged);
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
