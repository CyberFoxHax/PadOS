
using System;
using System.Linq;
using System.Windows;

namespace PadOS.Views.GamePadOSK {
	public partial class Osk  {
		public Osk(){
			System.Windows.Media.RenderOptions.SetBitmapScalingMode(this, System.Windows.Media.BitmapScalingMode.HighQuality);
			InitializeComponent();
			Background = System.Windows.Media.Brushes.Transparent;
			_gamePadWrapper = new GamePadWrapper(this);
			TextBox.Text = "";
			System.Windows.Controls.Canvas.SetLeft(Caret, 0);

			if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)) return;
			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e){
			var wrapper = _gamePadWrapper;
			wrapper.MoveLeftDown += OnMoveLeftDown;
			wrapper.MoveRightDown += OnMoveRightDown;
			wrapper.ChangeSymbolsDown += OnWrapperOnChangeSymbolsDown;
			wrapper.ChangeCaseDown += OnWrapperOnChangeCaseDown;
			wrapper.ChangeCaseUp += OnChangeCaseAndSymbolsUp;
			wrapper.ChangeSymbolsUp += OnChangeCaseAndSymbolsUp;
			wrapper.BlockPosChanged += WrapperOnBlockPosChanged;
			wrapper.CharPosChanged += WrapperOnCharPosChanged;
			wrapper.DeleteDown += WrapperOnDeleteDown;
			wrapper.SpaceDown += WrapperOnSpaceDown;
			wrapper.EnterDown += WrapperOnEnterDown;

			_keyboardInputSimulator.CaretChange += KeyboardInputSimulatorOnCaretChange;
			_keyboardInputSimulator.TextChanged += KeyboardInputSimulatorOnTextChanged;
		}


		private void WrapperOnEnterDown() => _keyboardInputSimulator.OnEnterButton();
		private void WrapperOnSpaceDown() => _keyboardInputSimulator.OnSpaceButton();
		private void WrapperOnDeleteDown() => _keyboardInputSimulator.OnDeleteButton();
		private void WrapperOnCharPosChanged(Vector2 value) => _keyboardInputSimulator.InsertText(Dial.GetChar(value));
		private void WrapperOnBlockPosChanged(Vector2 value) => Dial.SetBlockFocus(value);
		private void OnChangeCaseAndSymbolsUp() => Dial.SwitchLowercase();
		private void OnWrapperOnChangeCaseDown() => Dial.SwitchUppercase();
		private void OnWrapperOnChangeSymbolsDown() => Dial.SwitchSymbols();
		private void OnMoveRightDown() => _keyboardInputSimulator.CaretIndex++;
		private void OnMoveLeftDown() => _keyboardInputSimulator.CaretIndex--;

		private void KeyboardInputSimulatorOnCaretChange(int i) => CaretIndex = i;
		private void KeyboardInputSimulatorOnTextChanged(string s) => TextBox.Text = s;


		private readonly GamePadWrapper _gamePadWrapper;
		private readonly KeyboardInputSimulator _keyboardInputSimulator = new KeyboardInputSimulator();

		public int CaretIndex {
			get => _keyboardInputSimulator.CaretIndex;
			set
			{
				if (value <= -1 || value >= TextBox.Text.Length + 1) return;
				TextBox.CaretIndex = value;
				var rect = TextBox.GetRectFromCharacterIndex(value);
				System.Windows.Controls.Canvas.SetLeft(Caret, rect.X);
			}
		}
	}
}
