using PadOS.Commands.FunctionButtons;
using PadOS.Input;
using PadOS.Input.BlockNavigator;
using PadOS.Input.WpfGamePad;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using System.Windows.Shapes;

namespace PadOS.Views.MainPanelEditor {
    public partial class DialAreaControl : UserControl {
        public DialAreaControl() {
            InitializeComponent();

            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                return;

            CenterHighlight.Visibility = Visibility.Collapsed;
            HighlightKnob.Visibility = Visibility.Collapsed;

            CenterHighlight.Loaded += delegate {
                var parent = (FrameworkElement)CenterHighlight.Parent;
                _centerHighlightCanvasSize = new Vector2(
                    parent.ActualWidth,
                    parent.ActualHeight
                );
            };
        }

        public event Action<FrameworkElement, FunctionButton> ItemPicked;

        private int _activeButtonIndex = -1;
        private Tuple<FrameworkElement, FunctionButton>[] _buttons = new Tuple<FrameworkElement, FunctionButton>[8];
        private Vector2 _centerHighlightCanvasSize;

        public void Reset() {
            CenterHighlight.Visibility = Visibility.Collapsed;
            HighlightKnob.Visibility = Visibility.Collapsed;
        }

        public void LoadPanel(SaveData.Models.Profiles group = null) {
            var ctx = new SaveData.SaveData();
            var saveData = ctx.PanelButtons.Where(p => p.Profile.Id == group.Id);
            var elms = ButtonsCanvas.Children.OfType<CustomControls.AlphaSilhouetteImage>().ToArray();
            foreach (var data in saveData) {
                var functionButton = new FunctionButton {
                    ImageUri = new Uri("pack://application:,,,/PadOS;component/Resources/" + data.Function.ImageUrl),
                    Title = data.Function.Title,
                    Identifier = data.Function.Parameter,
                    FunctionType = data.Function.FunctionType
                };

                var elm = elms[data.Position];

                elm.Source = new System.Windows.Media.Imaging.BitmapImage(functionButton.ImageUri);
                _buttons[data.Position] = new Tuple<FrameworkElement, FunctionButton>(elm, functionButton);
            }

            const int segments = 8;
            const double segment = (Math.PI * 2) / segments;
            ButtonsCanvas.Loaded += delegate {
                for (var i = 0; i < segments; i++) {
                    Canvas.SetLeft(elms[i], Math.Cos(segment * i - segment * 2) * 270 + ButtonsCanvas.ActualWidth / 2);
                    Canvas.SetTop(elms[i], Math.Sin(segment * i - segment * 2) * 270 + ButtonsCanvas.ActualHeight / 2);

                    if (_buttons[i] != null)
                        continue;
                    elms[i].Source = null;
                    elms[i].Visibility = Visibility.Hidden;
                }
            };
        }

        private void OnActivated() {
            BlockNavigator.SetIsDisabled(ButtonsCanvas, true);
            WpfGamePad.AddThumbLeftChangeHandler(ButtonsCanvas, OnLeftThumbChanged);
            WpfGamePad.AddButtonADownHandler(ButtonsCanvas, OnButtonA);
            WpfGamePad.AddButtonBDownHandler(ButtonsCanvas, OnButtonB);
            WpfGamePad.SetRegistered(ButtonsCanvas, true);
            WpfGamePad.SetIsFocused(ButtonsCanvas, true);

            CenterHighlight.Visibility = Visibility.Visible;
            HighlightKnob.Visibility = Visibility.Collapsed;
            _activeButtonIndex = -1;

            Canvas.SetLeft(CenterHighlight, _centerHighlightCanvasSize.X / 2 - CenterHighlight.Width / 2);
            Canvas.SetTop(CenterHighlight, _centerHighlightCanvasSize.Y / 2 - CenterHighlight.Height / 2);
        }

        public void Disable() {
            BlockNavigator.SetIsDisabled(ButtonsCanvas, false);
            WpfGamePad.RemoveThumbLeftChangeHandler(ButtonsCanvas, OnLeftThumbChanged);
            WpfGamePad.RemoveButtonADownHandler(ButtonsCanvas, OnButtonA);
            WpfGamePad.RemoveButtonBDownHandler(ButtonsCanvas, OnButtonB);
            WpfGamePad.SetIsFocused(ButtonsCanvas, false);
            WpfGamePad.SetRegistered(ButtonsCanvas, false);

            CenterHighlight.Visibility = Visibility.Collapsed;
        }

        private void OnButtonA(object sender, GamePadEventArgs args) {
            if (_activeButtonIndex >= 0) {
                var funcButton = _buttons[_activeButtonIndex];
                ItemPicked?.Invoke(funcButton.Item1, funcButton.Item2);
            }
        }

        private void OnButtonB(object sender, GamePadEventArgs args) {
            Disable();
            BlockNavigator.NavigateBack(ButtonsCanvas);
        }

        private void OnLeftThumbChanged(object sender, GamePadEventArgs<Vector2> args) {
            var thumb = args.Value;
            if (thumb.GetLength() > 1)
                thumb /= thumb.GetLength();

            // don't know what i should call this one... It's just an equation thats redundant 2 times
            var somethingSize = new Vector2(
                _centerHighlightCanvasSize.X / 2 - CenterHighlight.Width / 2,
                _centerHighlightCanvasSize.Y / 2 - CenterHighlight.Height / 2
            );

            Canvas.SetLeft(CenterHighlight, somethingSize.X + thumb.X * somethingSize.X);
            Canvas.SetTop(CenterHighlight, somethingSize.Y + -thumb.Y * somethingSize.Y);

            if (thumb.GetLength() < 0.1) {
                Canvas.SetLeft(CenterHighlight, somethingSize.X);
                Canvas.SetTop(CenterHighlight, somethingSize.Y);
            }
            if (thumb.GetLength() < 0.2) {
                HighlightKnob.Visibility = Visibility.Collapsed;
                _activeButtonIndex = -1;
                return;
            }
            HighlightKnob.Visibility = Visibility.Visible;

            var angle = thumb.GetAngle() + (Math.PI * 2) / 16;
            var fract = angle / (Math.PI * 2);
            {
                var angleWrap = fract;
                if (angleWrap < 0)
                    angleWrap = angleWrap + 1;
                _activeButtonIndex = (int)(angleWrap * 8);
            }
            HighlightTransform.Angle = Math.Floor(fract * 8) / 8 * 360 + 90;
        }

        public void DialArea_ConfirmClick(object sender, EventArgs args) {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null)
                return;
            OnActivated();
        }
    }
}
