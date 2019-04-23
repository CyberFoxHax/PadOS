using PadOS.Input.BlockNavigator;
using PadOS.Navigation;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PadOS.Views.MainPanelEditor {
    public partial class MainPanelEditor{
		public MainPanelEditor() {
			InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
				return;

            DialAreaControl.ItemPicked += DialArea_ItemPicked;
            var ctx = new SaveData.SaveData();
            Profiles_ComboBox.ItemsSource = ctx.Profiles.Select(p=>p.Name).ToList();
            var profile = ctx.Profiles.First();
            var saveData = ctx.PanelButtons.Where(p => p.Profile.Id == profile.Id);
            DialAreaControl.LoadPanel(saveData);
            FunctionButtonList.LoadList(ctx.Functions.ToList());
        }

        private void DialArea_ItemPicked(FrameworkElement sender, FunctionViewModel obj) {
            DialAreaControl.Disable();
            BlockNavigator.SetFocus(sender, FunctionButtonList.GetFirstItemElement());
        }

        private void CreateNew_ButtonClick(object sender, MouseButtonEventArgs e){
            // change view to function creation
		}

        private void Window_CancelClick(object sender, EventArgs args) {
            Navigator.NavigateBack();
        }


        public void DialArea_ConfirmClick(object sender, EventArgs args) => DialAreaControl.DialArea_ConfirmClick(sender, args);

        private void FunctionButtonList_NavigationExit(object sender, EventArgs args) {
            DialAreaControl.Reset();
        }

        private void FunctionButtonList_ItemClicked(Commands.FunctionButtons.FunctionButton obj) {

        }

        private void FunctionButtonList_ItemClicked(object sender, FunctionViewModel model) {
            BlockNavigator.SetFocus(sender as FrameworkElement, DialAreaControl);
            BlockNavigator.EnterNestedNavigator(DialAreaControl);
            if (DialAreaControl.IsPicking) {
                DialAreaControl.ReplaceSelectedItem(model);
            }
            else {

            }
            DialAreaControl.Enable();
        }
    }
}
