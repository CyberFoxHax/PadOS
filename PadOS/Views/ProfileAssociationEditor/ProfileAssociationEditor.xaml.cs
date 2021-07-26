using PadOS.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
namespace PadOS.Views.ProfileAssociationEditor {
    public partial class ProfileAssociationEditor : Window {
        public ProfileAssociationEditor() {
            InitializeComponent();

            AssociationsListView.Visibility = Visibility.Collapsed;
            ItemEditView.Visibility = Visibility.Collapsed;

            ProfilesListView.Items.Clear();
            using (var data = new SaveData.SaveData())
                ProfilesListView.ItemsSource = data.Profiles.ToArray();
        }

        private SaveData.Models.Profile _selectedProfile;

        private void Window_CancelClick(object sender, EventArgs args) {
            Navigator.NavigateBack();
        }

        private void Button_ProfileHover(object sender, EventArgs args) {
            var s = (System.Windows.Controls.Border)sender;
            var data = (SaveData.Models.Profile)s.DataContext;
            TextBox_ProfileName.Text = data.Name;
        }

        private void ButtonProfile_Click(object sender, EventArgs args) {
            {
                var s = (System.Windows.Controls.Border)sender;
                var data = (SaveData.Models.Profile)s.DataContext;
                _selectedProfile = data;
            }

            AssociationsListView.ItemsSource = null;
            AssociationsListView.Items.Clear();
            using (var data = new SaveData.SaveData())
                AssociationsListView.ItemsSource = data.ProfileAssociations
                    .Where(p=>p.Profile.Id == _selectedProfile.Id)
                    .Select(p => {
                        var list = new List<string>();
                        if (string.IsNullOrEmpty(p.Executable)  == false) list.Add(p.Executable);
                        if (string.IsNullOrEmpty(p.WindowTitle) == false) list.Add(p.WindowTitle);

                        string title;
                        if (list.Count == 0)
                            title = "Everywhere";
                        else
                            title = string.Join("+", list);

                        return new ListItemData {
                            Title = title,
                            Data = p,
                        };
                    })
                    .ToArray();

            AssociationsListView.Visibility = Visibility.Visible;
            ItemEditView.Visibility = Visibility.Collapsed;
            Input.BlockNavigator.BlockNavigator.SetIsFocusable(EditPanel, true);
            Input.BlockNavigator.BlockNavigator.RefreshLayout(EditPanel);
            Input.BlockNavigator.BlockNavigator.SetFocus((FrameworkElement)sender, TextBox_ProfileName, true);
        }

        private void TextBoxProfile_ConfirmClick(object sender, EventArgs args) {
            // Launch text input
            var osk = new GamePadOSK.Osk();
            osk.HideLegend(true);
            osk.Show();
            osk.SetScale(0.3);
            

            var elm = (FrameworkElement)sender;
            var locationFromScreen = elm.PointToScreen(new Point(0, 0));
            var source = PresentationSource.FromVisual(this);
            var targetPoints = source.CompositionTarget.TransformFromDevice.Transform(locationFromScreen);
            var wpfWindow = osk;
            wpfWindow.Top = targetPoints.Y;
            wpfWindow.Left = targetPoints.X;

            osk.LayoutUpdated += (a, b) => {
                wpfWindow.Top = targetPoints.Y + elm.ActualHeight;
                wpfWindow.Left = targetPoints.X + elm.ActualWidth;
            };
        }

        private void ButtonCapture_Click(object sender, RoutedEventArgs e) {
            // Minimize everything
            // Begin Capture
        }

        private void ButtonPick_Click(object sender, RoutedEventArgs e) {
            // Open list
        }

        private void Button_RemoveOnClick(object sender, RoutedEventArgs e) {
            // Delete item from list
        }

        private FrameworkElement _lastEditButton;
        private async void Button_EditOnClick(object sender, RoutedEventArgs e) {
            AssociationsListView.Visibility = Visibility.Collapsed;
            ItemEditView.Visibility = Visibility.Visible;
            await Input.BlockNavigator.BlockNavigator.RefreshLayout(EditPanel);
            Input.BlockNavigator.BlockNavigator.SetFocus(TextBox_Exec, true);
            _lastEditButton = (FrameworkElement)sender;

            var data = (ListItemData)_lastEditButton.DataContext;
            TextBox_Exec.Text = data.Data.Executable;
            TextBox_Window.Text = data.Data.WindowTitle;
        }

        private async void EditPanel_CancelClick(object sender, RoutedEventArgs args) {
            if (AssociationsListView.Visibility == Visibility.Collapsed) {
                AssociationsListView.Visibility = Visibility.Visible;
                ItemEditView.Visibility = Visibility.Collapsed;
                args.Handled = true;
                await Input.BlockNavigator.BlockNavigator.RefreshLayout(EditPanel);
                Input.BlockNavigator.BlockNavigator.SetFocus(_lastEditButton);
            }
        }
    }
}
