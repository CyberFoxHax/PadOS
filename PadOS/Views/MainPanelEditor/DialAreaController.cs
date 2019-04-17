using PadOS.Commands.FunctionButtons;
using PadOS.Input;
using PadOS.Input.BlockNavigator;
using PadOS.Input.WpfGamePad;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;

namespace PadOS.Views.MainPanelEditor {
    public class DialAreaController {

        public DialAreaController(Canvas dialArea_ButtonsCanvas, RotateTransform highlightRotate, Grid highlight) {
            _dialArea_ButtonsCanvas = dialArea_ButtonsCanvas;
            _highlightRotate = highlightRotate;
            _highlight = highlight;

            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(_dialArea_ButtonsCanvas))
                return;

            highlight.Visibility = Visibility.Collapsed;
        }

        private Canvas _dialArea_ButtonsCanvas;
        private RotateTransform _highlightRotate;
        private Grid _highlight;
        private int _activeButtonIndex = -1;
        private FunctionButton[] _buttons = new FunctionButton[8];
        
        public void LoadPanel(SaveData.Models.Profiles group = null) {
            var ctx = new SaveData.SaveData();
            var saveData = ctx.PanelButtons.Where(p=>p.Profile.Id == group.Id);
            foreach (var data in saveData) {
                _buttons[data.Position] = new FunctionButton {
                    ImageUri = new Uri("pack://application:,,,/PadOS;component/Resources/" + data.Function.ThumbnailUri),
                    Title = data.Function.Title,
                    Key = data.Function.Parameter,
                    FunctionType = data.Function.FunctionType
                };
                SetButton(data.Position, _buttons[data.Position]);
            }

            var elms = _dialArea_ButtonsCanvas.Children.OfType<CustomControls.AlphaSilhouetteImage>().ToArray();
            const int upper = 8;
            const double tau = Math.PI * 2;
            const double segment = tau / upper;
            _dialArea_ButtonsCanvas.Loaded += delegate {
                for (var i = 0; i < upper; i++) {
                    Canvas.SetLeft(elms[i], Math.Cos(segment * i - segment * 2) * 270 + _dialArea_ButtonsCanvas.ActualWidth / 2);
                    Canvas.SetTop(elms[i], Math.Sin(segment * i - segment * 2) * 270 + _dialArea_ButtonsCanvas.ActualHeight / 2);

                    if (_buttons[i] != null)
                        continue;
                    elms[i].Source = null;
                    elms[i].Visibility = Visibility.Hidden;
                }
            };
        }

        public void SetButton(int index, FunctionButton button) {
            var elms = _dialArea_ButtonsCanvas.Children.OfType<CustomControls.AlphaSilhouetteImage>().ToArray();
            _buttons[index] = button;
            elms[index].Source = new System.Windows.Media.Imaging.BitmapImage(_buttons[index].ImageUri);
        }

        private void OnActivated() {
            BlockNavigator.SetIsDisabled(_dialArea_ButtonsCanvas, true);
            WpfGamePad.AddThumbLeftChangeHandler(_dialArea_ButtonsCanvas, OnLeftThumbChanged);
            WpfGamePad.AddButtonADownHandler(_dialArea_ButtonsCanvas, OnButtonA);
            WpfGamePad.AddButtonBDownHandler(_dialArea_ButtonsCanvas, OnButtonB);
            WpfGamePad.SetRegistered(_dialArea_ButtonsCanvas, true);
            WpfGamePad.SetIsFocused(_dialArea_ButtonsCanvas, true);
        }


        private void OnButtonA(object sender, GamePadEventArgs args) {
            if (_activeButtonIndex >= 0) {
                var funcButton = _buttons[_activeButtonIndex];
            }
        }

        private void OnButtonB(object sender, GamePadEventArgs args) {
            BlockNavigator.SetIsDisabled(_dialArea_ButtonsCanvas, true);
            WpfGamePad.RemoveThumbLeftChangeHandler(_dialArea_ButtonsCanvas, OnLeftThumbChanged);
            WpfGamePad.RemoveButtonADownHandler(_dialArea_ButtonsCanvas, OnButtonA);
            WpfGamePad.RemoveButtonBDownHandler(_dialArea_ButtonsCanvas, OnButtonB);
            WpfGamePad.SetIsFocused(_dialArea_ButtonsCanvas, false);
            WpfGamePad.SetRegistered(_dialArea_ButtonsCanvas, false);
        }

        private void OnLeftThumbChanged(object sender, GamePadEventArgs<Vector2> args) {
            var thumb = args.Value;
            if (thumb.GetLength() < 0.2) {
                _highlight.Visibility = Visibility.Collapsed;
                _activeButtonIndex = -1;
                return;
            }
            _highlight.Visibility = Visibility.Visible;

            var angle = thumb.GetAngle();
            var fract = angle / (Math.PI * 2);
            {
                var angleWrap = fract;
                if (angleWrap < 0)
                    angleWrap = angleWrap + 1;
                _activeButtonIndex = (int)(angleWrap * 8);
            }
            _highlightRotate.Angle = Math.Floor(fract * 8) / 8 * 360 + 90;
        }

        public void DialArea_ConfirmClick(object sender, EventArgs args) {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null)
                return;
            OnActivated();
        }

    }
}
