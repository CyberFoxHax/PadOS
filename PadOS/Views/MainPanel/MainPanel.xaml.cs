using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PadOS.Input;
using FunctionButton = PadOS.Commands.FunctionButtons.FunctionButton;

namespace PadOS.Views.MainPanel {
	public partial class MainPanel {
		public MainPanel() {
			InitializeComponent();
			Highlight.Visibility = Visibility.Hidden;

			IsVisibleChanged += OnIsVisibleChanged;

			var ctx = new SaveData.SaveData();
			var saveData = ctx.PanelButtons;
			foreach (var data in saveData){
				_buttons[data.Position] = new FunctionButton {
					ImageUri = new Uri("pack://application:,,,/PadOS;component/Resources/" + data.Function.ThumbnailUri),
					Title = data.Function.Title,
					Key = data.Function.Parameter,
					FunctionType = data.Function.FunctionType
				};
				SetButton(data.Position, _buttons[data.Position]);
			}

			var elms = Canvas.Children.OfType<CustomControls.AlphaSilhouetteImage>().ToArray();
			const int upper = 8;
			const double tau = Math.PI * 2;
			const double segment = tau/upper;
			for (var i = 0; i < upper; i++){
				Canvas.SetLeft(elms[i], Math.Cos(segment * i - segment * 2) * 270 + Width / 2);
				Canvas.SetTop (elms[i], Math.Sin(segment * i - segment * 2) * 270 + Height / 2);

				if (_buttons[i] != null) continue;
				elms[i].Source = null;
				elms[i].Visibility = Visibility.Hidden;
			}
		}

		public bool IsGamePadFocused { get; set; }
		private readonly FunctionButton[] _buttons = new FunctionButton[8];
		private bool _waitForReturnZero;

		public void SetButton(int index, FunctionButton button) {
			var elms = Canvas.Children.OfType<CustomControls.AlphaSilhouetteImage>().ToArray();
			_buttons[index] = button;
			elms[index].Source = 
				new System.Windows.Media.Imaging.BitmapImage(_buttons[index].ImageUri);
		}

		public void ClearButtons(){
			for (var i = 0; i < 8; i++)
				RemoveButton(i);
		}

		public void RemoveButton(int index) {
			var elms = Canvas.Children.OfType<CustomControls.AlphaSilhouetteImage>().ToArray();
			elms[index].Source = null;
			elms[index].Visibility = Visibility.Hidden;
			_buttons[index] = null;
		}

		private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
			Highlight.Visibility = Visibility.Hidden;
			_waitForReturnZero = false;
		}

		private void ActivateButton(int index){
			if (index >= _buttons.Length || _buttons[index] == null) return;
			_buttons[index].Exec();
			Hide();
			
		}

		private void GamepadInputOnThumbLeftChange(object sender, GamePadEventArgs<Input.Vector2> args){
			var length = args.Value.GetLength();
			var angle  = args.Value.GetAngle();
			if (length > 0.9 && _waitForReturnZero == false) {
				var angleWrap = angle < 0 ? Math.PI *2 + angle: angle;
				ActivateButton((int)Math.Round(angleWrap / Math.PI * 180 / 45));
				_waitForReturnZero = true;
			}
			else if(length > 0.2){
					HighlightRotate.Angle = Math.Round((angle / Math.PI * 180 + 90) / 45) * 45;
					Highlight.Visibility = Visibility.Visible;
			}
			else{
				_waitForReturnZero = false;
				Highlight.Visibility = Visibility.Hidden;
			}
		}
    }
}
