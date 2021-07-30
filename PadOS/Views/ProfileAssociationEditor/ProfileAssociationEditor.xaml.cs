using PadOS.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PadOS.Views.ProfileAssociationEditor {
    public partial class ProfileAssociationEditor : Window {
        public ProfileAssociationEditor() {
            InitializeComponent();

            AssociationsStackPanel.Visibility = Visibility.Collapsed;
            ItemEditView.Visibility = Visibility.Collapsed;

            AssociationsListView.ItemsSource = null;
            AssociationsListView.Items.Clear();

            _listViewData = new System.Collections.ObjectModel.ObservableCollection<ListItemData>();
            AssociationsListView.ItemsSource = _listViewData;
            LoadData();
        }

        private SaveData.Models.Profile _selectedProfile;
        private Dictionary<SaveData.Models.Profile, List<SaveData.Models.ProfileAssociation>> _profileAssociations
            = new Dictionary<SaveData.Models.Profile, List<SaveData.Models.ProfileAssociation>>();

        private void Window_CancelClick(object sender, EventArgs args) {
            Navigator.NavigateBack();
        }

        private void Button_ProfileHover(object sender, EventArgs args) {
            var s = (System.Windows.Controls.Border)sender;
            var data = (SaveData.Models.Profile)s.DataContext;
            TextBox_ProfileName.Text = data.Name;
        }

        private void LoadData() {
            ProfilesListView.Items.Clear();
            using (var data = new SaveData.SaveData()) {
                ProfilesListView.ItemsSource = data.Profiles.Where(p=>p.Id != 1 && p.Id != 2).ToArray();
                foreach (var profile in data.Profiles)
                    _profileAssociations[profile] = data.ProfileAssociations.Where(p => p.Profile.Id == profile.Id).ToList();
            }
        }

        private void RefreshListView(IEnumerable<SaveData.Models.ProfileAssociation> data = null) {
            if (data == null)
                data = _profileAssociations[_selectedProfile];
            var trout = data
                .Select(p => {
                    var list = new List<string>();
                    if (string.IsNullOrEmpty(p.Executable) == false) list.Add(p.Executable);
                    if (string.IsNullOrEmpty(p.WindowTitle) == false) list.Add(p.WindowTitle);

                    string title;
                    if(p.Id == 0)
                        title = "";
                    else if (list.Count == 0)
                        title = "Everywhere";
                    else
                        title = string.Join("+", list);

                    return new ListItemData {
                        Title = title,
                        Data = p,
                    };
                })
                .ToArray();

            _listViewData.Clear();
            foreach (var item in trout) {
                _listViewData.Add(item);
            }
            AssociationsListView.Items.Refresh();
        }

        private async void ButtonProfile_Click(object sender, EventArgs args) {
            {
                var s = (System.Windows.Controls.Border)sender;
                var data = (SaveData.Models.Profile)s.DataContext;
                _selectedProfile = data;
            }
            RefreshListView();

            AssociationsStackPanel.Visibility = Visibility.Visible;
            ItemEditView.Visibility = Visibility.Collapsed;
            //Input.BlockNavigator.BlockNavigator.SetIsFocusable(EditPanel, true);
            Input.BlockNavigator.BlockNavigator.RefreshLayout(ListPanel);
            Input.BlockNavigator.BlockNavigator.SetFocus((FrameworkElement)sender, TextBox_ProfileName, true);
        }

        private void TextBox_ConfirmClick(object sender, EventArgs args) {
            // Launch text input
            var osk = new GamePadOSK.Osk(false) {
                Width = 0,
                Height = 0,
                Top = -1000,
                Left = -1000,
            };
            osk.HideLegend(true);
            osk.Show();
            osk.SetScale(0.3);

            var elm = (FrameworkElement)sender;
            var locationFromScreen = elm.PointToScreen(new Point(0, 0));
            var source = PresentationSource.FromVisual(this);
            var targetPoints = source.CompositionTarget.TransformFromDevice.Transform(locationFromScreen);

            osk.LayoutUpdated += (a, b) => { // remember unsub
                var sin = Math.Sin(Math.PI/4)* osk.ActualWidth / 2;
                osk.Top = targetPoints.Y + elm.ActualHeight -osk.ActualWidth/2 + sin;
                osk.Left = targetPoints.X + elm.ActualWidth -osk.ActualWidth/2 + sin;
            };

            var text = (System.Windows.Controls.TextBox)sender;
            text.CaretIndex = text.Text.Length;
            text.Focus();
            osk.SetText(text.Text);
            osk.Topmost = true;
            osk.EnterClick += s => {
                Input.BlockNavigator.BlockNavigator.SetIsDisabled(text, false);
                osk.Close();
                osk.Dispose();
            };
            Input.BlockNavigator.BlockNavigator.SetIsDisabled(text, true);
            osk.TextChanged += (s, p) => {
                text.Text = p;
                text.CaretIndex = s.CaretIndex;
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

        private ListItemData _selectedProfileAssociation;
        private System.Collections.ObjectModel.ObservableCollection<ListItemData> _listViewData;

        private async void Button_EditOnClick(object sender, RoutedEventArgs e) {
            AssociationsStackPanel.Visibility = Visibility.Collapsed;
            ItemEditView.Visibility = Visibility.Visible;
            await Input.BlockNavigator.BlockNavigator.RefreshLayout(ListPanel);
            Input.BlockNavigator.BlockNavigator.SetFocus(TextBox_Exec, true);
            _selectedProfileAssociation = (sender as FrameworkElement).DataContext as ListItemData;

            TextBox_Exec.Text = _selectedProfileAssociation.Data.Executable;
            TextBox_Window.Text = _selectedProfileAssociation.Data.WindowTitle;
        }

        private async void EditPanel_CancelClick(object sender, RoutedEventArgs args) {
            if (AssociationsStackPanel.Visibility == Visibility.Visible)
                return;
            args.Handled = true;

            AssociationsStackPanel.Visibility = Visibility.Visible;
            ItemEditView.Visibility = Visibility.Collapsed;
            await Input.BlockNavigator.BlockNavigator.RefreshLayout(ListPanel);
            Input.BlockNavigator.BlockNavigator.SetFocus(TextBox_ProfileName);

            var assocData = _selectedProfileAssociation.Data;
            if (assocData == null
                && string.IsNullOrEmpty(TextBox_Exec.Text)
                && string.IsNullOrEmpty(TextBox_Window.Text))
                return;
            if(assocData != null
                && assocData.Executable == TextBox_Exec.Text
                && assocData.WindowTitle == TextBox_Window.Text)
                return;

            if (assocData == null) {
                assocData = new SaveData.Models.ProfileAssociation {
                    Profile = _selectedProfile
                };
                _selectedProfileAssociation.Data = assocData;
                _profileAssociations[_selectedProfile].Add(assocData);
            }
            assocData.Executable = TextBox_Exec.Text;
            assocData.WindowTitle = TextBox_Window.Text;

            using (var db = new SaveData.SaveData()) {
                db.ProfileAssociations.UpdateOrInsert(assocData);
                db.SaveChanges();
            }
            RefreshListView();
            await Input.BlockNavigator.BlockNavigator.RefreshLayout(ListPanel);

        }

        private async void Button_NewOnClick(object sender, RoutedEventArgs e) {
            AssociationsStackPanel.Visibility = Visibility.Collapsed;
            ItemEditView.Visibility = Visibility.Visible;
            await Input.BlockNavigator.BlockNavigator.RefreshLayout(ListPanel);
            Input.BlockNavigator.BlockNavigator.SetFocus(TextBox_Exec, true);

            _selectedProfileAssociation = new ListItemData();
            TextBox_Exec.Text = "";
            TextBox_Window.Text = "";
        }

        private void AddNewProfileClick(object sender, RoutedEventArgs e) {

        }
    }
}
