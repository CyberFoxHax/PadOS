using PadOS.Commands.FunctionButtons;
using PadOS.Input;
using PadOS.Input.BlockNavigator;
using PadOS.Input.WpfGamePad;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

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

        public delegate void ItemPickedEvent(FrameworkElement sender, FunctionViewModel pickedFunction);
        public event ItemPickedEvent ItemPicked;

        private int _activeButtonIndex = -1;
        private FunctionViewModel[] _buttons = new FunctionViewModel[8];
        private Vector2 _centerHighlightCanvasSize;

        public FunctionViewModel Selection {
            get {
                if (_activeButtonIndex < 0)
                    return null;
                return _buttons[_activeButtonIndex];
            }
        }

        public void Reset() {
            CenterHighlight.Visibility = Visibility.Collapsed;
            HighlightKnob.Visibility = Visibility.Collapsed;
        }

        public void LoadPanel(System.Collections.Generic.IEnumerable<SaveData.Models.PanelButton> saveData) {
            var elms = ButtonsCanvas.Children.OfType<CustomControls.AlphaSilhouetteImage>().ToArray();
            foreach (var data in saveData) {
                var functionButton = new FunctionButton {
                    ImageUri = new Uri(Utils.ResourcesPath + data.Function.ImageUrl),
                    Title = data.Function.Title,
                    Identifier = data.Function.Parameter,
                    FunctionType = data.Function.FunctionType
                };

                var elm = elms[data.Position];

                elm.Source = new System.Windows.Media.Imaging.BitmapImage(functionButton.ImageUri);
                _buttons[data.Position] = new FunctionViewModel {
                    FrameworkElement = elm, 
                    FunctionButton = functionButton
                };
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
                    elms[i].Visibility = Visibility.Collapsed;
                }
            };
        }

        public void ReplaceSelectedItem(FunctionViewModel model) {
            var elms = ButtonsCanvas.Children.OfType<CustomControls.AlphaSilhouetteImage>().ToArray();
            var elm = elms[_activeButtonIndex];
            if (model.FunctionButton.ImageUri != null) {
                elm.Source = new System.Windows.Media.Imaging.BitmapImage(model.FunctionButton.ImageUri);
                elm.Visibility = Visibility.Visible;
            }
            else
                elm.Visibility = Visibility.Collapsed;

            _buttons[_activeButtonIndex] = new FunctionViewModel {
                FrameworkElement = elm,
                FunctionButton = model.FunctionButton,
                Function = model.Function
            };
        }

        public void Enable() {
            BlockNavigator.SetIsDisabled(this, true);
            WpfGamePad.AddThumbLeftChangeHandler(this, OnLeftThumbChanged);
            WpfGamePad.AddButtonADownHandler(this, OnButtonA);
            WpfGamePad.AddButtonBDownHandler(this, OnButtonB);
            WpfGamePad.SetRegistered(this, true);
            WpfGamePad.SetIsFocused(this, true);

            CenterHighlight.Visibility = Visibility.Visible;
            HighlightKnob.Visibility = Visibility.Collapsed;
            _activeButtonIndex = -1;

            Canvas.SetLeft(CenterHighlight, _centerHighlightCanvasSize.X / 2 - CenterHighlight.Width / 2);
            Canvas.SetTop(CenterHighlight, _centerHighlightCanvasSize.Y / 2 - CenterHighlight.Height / 2);
        }

        public void Disable() {
            BlockNavigator.SetIsDisabled(this, false);
            WpfGamePad.RemoveThumbLeftChangeHandler(this, OnLeftThumbChanged);
            WpfGamePad.RemoveButtonADownHandler(this, OnButtonA);
            WpfGamePad.RemoveButtonBDownHandler(this, OnButtonB);
            WpfGamePad.SetIsFocused(this, false);
            WpfGamePad.SetRegistered(this, false);

            CenterHighlight.Visibility = Visibility.Collapsed;
        }

        private void OnButtonA(object sender, GamePadEventArgs args) {
            if (_activeButtonIndex < 0)
                return;
            
            var model = _buttons[_activeButtonIndex];

            if(model != null)
                ItemPicked?.Invoke(model.FrameworkElement, model);
            else {
                var elms = ButtonsCanvas.Children.OfType<CustomControls.AlphaSilhouetteImage>().ToArray();
                ItemPicked?.Invoke(elms[_activeButtonIndex], null);
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
            Enable();
        }
    }
}
