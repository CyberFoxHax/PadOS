using System;
using System.Linq;
using System.Windows;

namespace PadOS.Views.GamePadOSK {
	public partial class Osk  {
        public Osk() : this(true) {

        }
		public Osk(bool simulate){
            System.Windows.Media.RenderOptions.SetBitmapScalingMode(this, System.Windows.Media.BitmapScalingMode.HighQuality);
            InitializeComponent();
			_gamePadWrapper = new GamePadWrapper(this);
			TextBox.Text = "";
			System.Windows.Controls.Canvas.SetLeft(Caret, 0);

			if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)) return;
			Loaded += OnLoaded;

            _startSize = new Input.Vector2(
                Width,
                Height
            );

            _keyboardInputSimulator.SimulatorKeyboard = simulate;
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
            wrapper.HideLegend += Wrapper_HideLegend;
            wrapper.OnScale += Wrapper_OnScale;
            wrapper.OnMove += Wrapper_OnMove;

			_keyboardInputSimulator.CaretChange += KeyboardInputSimulatorOnCaretChange;
			_keyboardInputSimulator.TextChanged += KeyboardInputSimulatorOnTextChanged;
        }

        public void Dispose() {
            _gamePadWrapper.Dispose();
        }

        /*public SimpleEventSimulator UseEventSimulator() {
            var sim = new SimpleEventSimulator();
            _keyboardInputSimulator = sim;
            return sim;
        }*/

        public void SetText(string s) {
            _keyboardInputSimulator.SetText(s);
            TextBox.Text = s;
        }

        public void SetScale(double scale) {
            _currentScale = scale;
            var height = _startSize.Y * _currentScale;
            var width = _startSize.X * _currentScale;
            Height = height;
            Width = width;
        }

        private double _currentScale = 1;
        private Input.Vector2 _startSize;
        private const float Speed = 20;

        private void Wrapper_OnMove(Input.Vector2 dir) {
            // move
            var top = Top + -dir.Y * Speed;
            var left = Left + dir.X * Speed;
            
            if (BorderLegendArea.Visibility == System.Windows.Visibility.Visible) {
                if (top + ActualHeight > System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height)
                    Top = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - Height;
                else if (top < 0)
                    Top = 0;
                else
                    Top = top;

                if (left + ActualWidth > System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width)
                    Left = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - Width;
                else if (left < 0)
                    Left = 0;
                else
                    Left = left;
            }
            else {
                if (top + Dial.ActualHeight * _currentScale > System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height)
                    Top = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - Dial.ActualHeight * _currentScale;
                else if (top < 0)
                    Top = 0;
                else
                    Top = top;

                var helperToInputControlSizeDiff = (BorderLegendArea.ActualWidth - BorderLegendArea.ActualWidth) / 2 * _currentScale;

                if (left + helperToInputControlSizeDiff + Dial.ActualWidth * _currentScale > System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width)
                    Left = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - helperToInputControlSizeDiff - Dial.ActualWidth * _currentScale;
                else if (left < -helperToInputControlSizeDiff)
                    Left = -helperToInputControlSizeDiff;
                else
                    Left = left;
            }
        }

        private void Wrapper_OnScale(double v) {
            _currentScale += v / 10;
            if (_currentScale < 0)
                _currentScale = 0;
            else if (_currentScale > 1.36f)
                _currentScale = 1.36f;

            var height = _startSize.Y * _currentScale;
            var heightDiff = height - Height;
            Height = height;
            Top -= heightDiff * 0.5;

            var width = _startSize.X * _currentScale;
            var widthDiff = width - Width;
            Width = width;
            Left -= widthDiff * 0.55;
        }

        public void HideLegend(bool v) {
            if (v)
                BorderLegendArea.Visibility = Visibility.Collapsed;
            else
                BorderLegendArea.Visibility = Visibility.Visible;
        }

        private void Wrapper_HideLegend() {
            HideLegend(BorderLegendArea.Visibility == Visibility.Visible);
        }

        private void WrapperOnEnterDown() {
            EnterClick?.Invoke(this);
            _keyboardInputSimulator.OnEnterButton();
        }
		private void WrapperOnSpaceDown() => _keyboardInputSimulator.OnSpaceButton();
		private void WrapperOnDeleteDown() => _keyboardInputSimulator.OnDeleteButton();
		private void WrapperOnCharPosChanged(Input.Vector2 value) => _keyboardInputSimulator.InsertText(Dial.GetChar(value));
		private void WrapperOnBlockPosChanged(Input.Vector2 value) => Dial.SetBlockFocus(value);
		private void OnChangeCaseAndSymbolsUp() => Dial.SwitchLowercase();
		private void OnWrapperOnChangeCaseDown() => Dial.SwitchUppercase();
		private void OnWrapperOnChangeSymbolsDown() => Dial.SwitchSymbols();
		private void OnMoveRightDown() => _keyboardInputSimulator.CaretIndex++;
		private void OnMoveLeftDown() => _keyboardInputSimulator.CaretIndex--;

		private void KeyboardInputSimulatorOnCaretChange(int i) => CaretIndex = i;
        private void KeyboardInputSimulatorOnTextChanged(string s) {
            TextBox.Text = s;
            TextChanged?.Invoke(this, s);
        }

        public event Action<Osk> EnterClick;
        public event Action<Osk, string> TextChanged;

        private readonly GamePadWrapper _gamePadWrapper;
		private KeyboardInputSimulator _keyboardInputSimulator = new KeyboardInputSimulator();

		public int CaretIndex {
			get => _keyboardInputSimulator.CaretIndex;
			set {
                if (BorderLegendArea.Visibility != Visibility.Visible || value <= -1 || value >= TextBox.Text.Length + 1) {
                    TextChanged?.Invoke(this, _keyboardInputSimulator.Text);
                    return;
                }
				TextBox.CaretIndex = value;
				var rect = TextBox.GetRectFromCharacterIndex(value);
				System.Windows.Controls.Canvas.SetLeft(Caret, rect.X);
                TextChanged?.Invoke(this, _keyboardInputSimulator.Text);
			}
		}
	}
}
