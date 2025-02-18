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

        private bool _profileAssociationHasChanged;
        private bool _profileChanged;
        private SaveData.Models.Profile _selectedProfile;
        private List<SaveData.Models.ProfileAssociation> currentProfileAssociations => _profileAssociations[_selectedProfile];
        private Dictionary<SaveData.Models.Profile, List<SaveData.Models.ProfileAssociation>> _profileAssociations
            = new Dictionary<SaveData.Models.Profile, List<SaveData.Models.ProfileAssociation>>();
        private List<SaveData.Models.ProfileAssociation> _deletedProfileAssociations = new List<SaveData.Models.ProfileAssociation>();

        private async void Window_CancelClick(object sender, EventArgs args) {
            if (_profileChanged) {
                Input.BlockNavigator.BlockNavigator.SetIsDisabled(this, true);
                var res = await CustomControls.ConfirmDialogue.ShowDialogAsync();
                if (res == CustomControls.ConfirmDialogue.DialogueResult.Cancel) {
                    Input.BlockNavigator.BlockNavigator.SetIsDisabled(this, false);
                    return;
                }

                if (res == CustomControls.ConfirmDialogue.DialogueResult.Yes) {
                    using (var db = new SaveData.SaveData()) {
                        db.ProfileAssociations.UpdateOrInsert(_profileAssociations.Values.SelectMany(p => p));
                        db.ProfileAssociations.RemoveRange(_deletedProfileAssociations);
                        db.SaveChanges();
                    }
                }
            }

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

                    string title = string.Join("+", list);

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
            osk.SetScale(0.5);

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

        private static string GetForegroundWindowProcess() {
            var hWnd = DllImport.UserInfo32.GetForegroundWindow();

            int processId;
            DllImport.UserInfo32.GetWindowThreadProcessId(hWnd, out processId);
            if (processId == 0)
                return null;

            var current = System.Diagnostics.Process.GetCurrentProcess();

            var firstOrDefault = System.Diagnostics.Process.GetProcesses().FirstOrDefault(p => p.Id == processId);
            if (firstOrDefault == null)
                return null;
            var newProcess = ProfileSwitcher.BackgroundTracker.GetMainModuleFileName(firstOrDefault);
            return newProcess;
        }

        private static string GetForegroundWindowTitle() {
            var hWnd = DllImport.UserInfo32.GetForegroundWindow();
            var sb = new System.Text.StringBuilder(128);
            var code = DllImport.UserInfo32.GetWindowText(hWnd, sb, sb.Capacity);
            return sb.ToString();
        }

        private void ButtonCaptureExec_Click(object sender, RoutedEventArgs e) {
            var process = GetForegroundWindowProcess();
            if (string.IsNullOrEmpty(TextBox_Exec.Text) || System.IO.Path.IsPathRooted(TextBox_Exec.Text)) {
                process = System.IO.Path.GetFileName(process);
            }
            TextBox_Exec.Text = process;
        }

        private void ButtonClearExec_Click(object sender, RoutedEventArgs e) {
            TextBox_Exec.Text = "";
        }

        private void ButtonCaptureWindow_Click(object sender, RoutedEventArgs e) {
            var process = GetForegroundWindowTitle();
            TextBox_Window.Text = process;
        }

        private void ButtonClearWindow_Click(object sender, RoutedEventArgs e) {
            TextBox_Window.Text = "";
        }


        private async void Button_RemoveOnClick(object sender, RoutedEventArgs e) {
            var item = (FrameworkElement)sender;
            var data = (ListItemData)item.DataContext;
            _deletedProfileAssociations.Add(data.Data);
            _profileAssociations[_selectedProfile].Remove(data.Data);
            RefreshListView();
            await Input.BlockNavigator.BlockNavigator.RefreshLayout(ListPanel);
            Input.BlockNavigator.BlockNavigator.SetFocus(TextBox_ProfileName);
            _profileChanged = true;
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

        private void ButtonSave_Click(object sender, RoutedEventArgs e) {
            EditAssociationExit();
            if(_profileAssociationHasChanged)
                _profileChanged = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) {
            _profileAssociationHasChanged = false;
            EditAssociationExit();
        }

        private void EditPanel_CancelClick(object sender, RoutedEventArgs args) {
            if (AssociationsStackPanel.Visibility == Visibility.Visible) {
                return;
            }
            args.Handled = true;
            EditAssociationExit();
        }

        private async void EditAssociationExit() {
            AssociationsStackPanel.Visibility = Visibility.Visible;
            ItemEditView.Visibility = Visibility.Collapsed;

            if (string.IsNullOrEmpty(TextBox_Exec.Text) == false || string.IsNullOrEmpty(TextBox_Window.Text) == false) {
                var assocData = _selectedProfileAssociation.Data;
                _profileAssociationHasChanged = assocData.Executable != TextBox_Exec.Text || assocData.WindowTitle != TextBox_Window.Text;
                assocData.Executable = string.IsNullOrEmpty(TextBox_Exec.Text) ? null : TextBox_Exec.Text;
                assocData.WindowTitle = string.IsNullOrEmpty(TextBox_Window.Text) ? null : TextBox_Window.Text;

                if (currentProfileAssociations.IndexOf(assocData) == -1) {
                    _profileAssociationHasChanged = true;
                    currentProfileAssociations.Add(assocData);
                }
            }

            RefreshListView();
            await Input.BlockNavigator.BlockNavigator.RefreshLayout(ListPanel);
            Input.BlockNavigator.BlockNavigator.NavigateBack(this);
            Input.BlockNavigator.BlockNavigator.SetFocus(TextBox_ProfileName);
        }

        private async void Button_NewOnClick(object sender, RoutedEventArgs e) {
            AssociationsStackPanel.Visibility = Visibility.Collapsed;
            ItemEditView.Visibility = Visibility.Visible;
            await Input.BlockNavigator.BlockNavigator.RefreshLayout(ListPanel);
            Input.BlockNavigator.BlockNavigator.SetFocus(TextBox_Exec, true);

            _selectedProfileAssociation = new ListItemData {
                Data = new SaveData.Models.ProfileAssociation {
                    Profile = _selectedProfile
                },
            };

            TextBox_Exec.Text = "";
            TextBox_Window.Text = "";
        }
    }
}
