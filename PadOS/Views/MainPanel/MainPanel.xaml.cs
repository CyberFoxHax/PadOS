using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PadOS.Views.MainPanel {
	public partial class MainPanel : Input.IGamePadFocusable{
		public MainPanel() {
			InitializeComponent();
			Highlight.Visibility = Visibility.Hidden;

			Input.WPFGamepad.Focus(this);
			var gamepadInput = Input.WPFGamepad.Register(this);
			gamepadInput.ThumbLeftChange += GamepadInputOnThumbLeftChange;
			Input.WPFGamepad.XInput.ButtonGuideDown += XInputOnButtonGuideDown;

			var saveData = SaveData.MainPanel.Data;
			foreach (var data in saveData.Items){
				_buttons[data.Position] = new MainPanelButton{
					ImageUri = "pack://application:,,,/PadOS;component/Resources/" + data.ImageUri,
					Key = data.Key
				};
				SetButton(data.Position, _buttons[data.Position]);
			}

			var elms = Canvas.Children.OfType<CustomControls.AlphaSilhouetteImage>().ToArray();
			const int upper = 8;
			const double tau = Math.PI * 2;
			const double segment = tau/upper;
			for (var i = 0; i < upper; i++){
				Canvas.SetLeft(elms[i], Math.Cos(segment * i - segment * 2) * 270 + Width / 2);
				Canvas.SetTop (elms[i], Math.Sin(segment * i - segment * 2) * 270 + Width / 2);

				if (_buttons[i] != null) continue;
				elms[i].Source = null;
				elms[i].Visibility = Visibility.Hidden;
			}
		}

		public bool IsGamePadFocused { get; set; }
		private readonly MainPanelButton[] _buttons = new MainPanelButton[8];
		private bool _waitForReturnZero;

		public void SetButton(int index, MainPanelButton button) {
			var elms = Canvas.Children.OfType<CustomControls.AlphaSilhouetteImage>().ToArray();
			_buttons[index] = button;
			elms[index].Source = 
				new System.Windows.Media.Imaging.BitmapImage(new Uri(_buttons[index].ImageUri, UriKind.Absolute));
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

		private void ActivateButton(int index){
			if (_buttons[index] == null) return;
			_buttons[index].Activate();
			Hide();
			Highlight.Visibility = Visibility.Hidden;
		}

		private void GamepadInputOnThumbLeftChange(object sender, Input.WPFGamepad.GamePadEventArgs<Vector2> args){
			var length = args.Value.GetLength();
			var angle  = args.Value.GetAngle();
			Dispatcher.BeginInvoke(new Action(() =>{
				if (length > 0.9 && _waitForReturnZero == false) {
					var angleWrap = angle < 0 ? Math.PI *2 + angle: angle;
					ActivateButton((int)Math.Round(angleWrap / Math.PI * 180 / 45));
					_waitForReturnZero = true;
				}
				else if(length > 0.1){
						HighlightRotate.Angle = Math.Round((angle / Math.PI * 180 + 90) / 45) * 45;
						Highlight.Visibility = Visibility.Visible;
				}
				else{
					_waitForReturnZero = false;
					Highlight.Visibility = Visibility.Hidden;
				}
			}));
		}

		private void XInputOnButtonGuideDown(XInputDotNetPure.PlayerIndex player, XInputDotNetPure.GamePadState state) {
			Dispatcher.BeginInvoke(new Action(() => {
				if (IsVisible) // todo how about other windows?
					Hide();
				else
					Show();
			}));
		}
	}
}
