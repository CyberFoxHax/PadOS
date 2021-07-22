using PadOS.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
namespace PadOS.Views.ProfileAssociationEditor {
    public partial class ProfileAssociationEditor : Window {
        public ProfileAssociationEditor() {
            InitializeComponent();

            ItemsListView.Visibility = Visibility.Collapsed;
            ItemEditView.Visibility = Visibility.Collapsed;
        }

        private void Window_CancelClick(object sender, EventArgs args) {
            Navigator.NavigateBack();
        }

        private void ButtonProfile_Click(object sender, EventArgs args) {
            ItemsListView.Visibility = Visibility.Visible;
            ItemEditView.Visibility = Visibility.Collapsed;
            Input.BlockNavigator.BlockNavigator.SetIsFocusable(EditPanel, true);
            Input.BlockNavigator.BlockNavigator.RefreshLayout(ItemsListView);
            Input.BlockNavigator.BlockNavigator.SetFocus((FrameworkElement)sender, TextBox_ProfileName, true);
        }

        private void TextBoxProfile_ConfirmClick(object sender, EventArgs args) {
            // Launch text input
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
            ItemsListView.Visibility = Visibility.Collapsed;
            ItemEditView.Visibility = Visibility.Visible;
            await Input.BlockNavigator.BlockNavigator.RefreshLayout(EditPanel);
            Input.BlockNavigator.BlockNavigator.SetFocus(TextBox_Exec, true);
            _lastEditButton = (FrameworkElement)sender;
        }

        private void Button_ProfileHover(object sender, EventArgs args) {
            var s = (System.Windows.Controls.Border)sender;
            TextBox_ProfileName.Text = ((s.Child as System.Windows.Controls.WrapPanel).Children[0] as System.Windows.Controls.TextBlock).Text;
        }

        private async void EditPanel_CancelClick(object sender, RoutedEventArgs args) {
            if (ItemsListView.Visibility == Visibility.Collapsed) {
                ItemsListView.Visibility = Visibility.Visible;
                ItemEditView.Visibility = Visibility.Collapsed;
                args.Handled = true;
                await Input.BlockNavigator.BlockNavigator.RefreshLayout(EditPanel);
                Input.BlockNavigator.BlockNavigator.SetFocus(_lastEditButton);
            }
        }
    }
}
