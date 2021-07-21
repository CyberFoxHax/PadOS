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
            //Panel_Editor.Visibility = Visibility.Collapsed;
        }

        private void Window_CancelClick(object sender, EventArgs args) {
            Navigator.NavigateBack();
        }

        private void ButtonProfile_Click(object sender, EventArgs args) {
            ItemsListView.Visibility = Visibility.Visible;
            //Panel_Editor.Visibility = Visibility.Visible;
            //Input.BlockNavigator.BlockNavigator.RefreshLayout(this);
            Input.BlockNavigator.BlockNavigator.RefreshLayout(ItemsListView);
            Input.BlockNavigator.BlockNavigator.SetFocus((FrameworkElement)sender, Panel_Editor, true);
        }

        private void TextBoxProfile_ConfirmClick(object sender, EventArgs args) {
            // Launch text input
        }

        private void ButtonCapture_Click(object sender, RoutedEventArgs e) {

        }

        private void ButtonPick_Click(object sender, RoutedEventArgs e) {

        }
    }
}
