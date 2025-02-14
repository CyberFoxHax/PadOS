using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PadOS.Input;
using FunctionButton = PadOS.Commands.FunctionButtons.FunctionButton;

namespace PadOS.Views.CircleDial {
	public partial class CircleDial: Navigation.IHideable
    {
		public CircleDial() {
			InitializeComponent();
			Highlight.Visibility = Visibility.Hidden;

			IsVisibleChanged += OnIsVisibleChanged;

            _buttonElements = Canvas.Children.OfType<PadOS.CustomControls.AlphaSilhouetteImage>().ToArray();

            LoadPanelData();

            var elms = _buttonElements;
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
        private PadOS.CustomControls.AlphaSilhouetteImage[] _buttonElements;

        public void LoadPanelData() {
            var dict = new System.Collections.Generic.Dictionary<int, SaveData.Models.PanelButton>();

            using (var ctx = new SaveData.SaveData()) {
                var currentProfile = Navigation.Navigator.GetCurrentProfile();
                var sharedButtons = ctx.PanelButtons.Where(p => p.Profile.Id == SaveData.DefaultData.AllProfile.Id).ToArray();
                var currentButtons = ctx.PanelButtons.Where(p => p.Profile.Id == currentProfile.Id).ToArray();
                foreach (var item in sharedButtons)
                    dict[item.Position] = item;

                foreach (var item in currentButtons)
                    dict[item.Position] = item;
            }

            for (int i = 0; i < 8; i++) {
                if (dict.ContainsKey(i)) {
                    var data = dict[i];
				    _buttons[data.Position] = new FunctionButton {
					    ImageUri = new Uri(Utils.ResourcesPath + data.Function.ImageUrl),
					    Title = data.Function.Title,
					    Identifier = data.Function.Parameter,
					    FunctionType = data.Function.FunctionType
				    };
                    _buttonElements[i].Source = new System.Windows.Media.Imaging.BitmapImage(_buttons[i].ImageUri);
                    _buttonElements[i].Visibility = Visibility.Visible;
                }
                else {
                    _buttonElements[i].Source = null;
                    _buttonElements[i].Visibility = Visibility.Hidden;
                    _buttons[i] = null;
                }
            }
        }

        async void Navigation.IHideable.Hide() {
            Opacity = 0;
            await System.Threading.Tasks.Task.Delay(50);
            Hide();
        }

        private async void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
            if (Visibility != Visibility.Visible) {
                return;
            }

            Highlight.Visibility = Visibility.Hidden;
			_waitForReturnZero = false;

            var name = Navigation.Navigator.GetCurrentProfile().Name;
            Txt_ProfileName.Text = name;
            Txt_ProfileName_Shadow1.Text = name;
            Txt_ProfileName_Shadow2.Text = name;
            Txt_ProfileName_Shadow3.Text = name;
            Txt_ProfileName_Shadow4.Text = name;

            LoadPanelData();

            Opacity = 1;
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
