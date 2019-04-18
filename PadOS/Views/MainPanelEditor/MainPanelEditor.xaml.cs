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
            DialAreaControl.LoadPanel(ctx.Profiles.First());
        }

        private void DialArea_ItemPicked(FrameworkElement sender, Commands.FunctionButtons.FunctionButton obj) {
            DialAreaControl.Disable();
            BlockNavigator.SetFocus(sender, FunctionButtonsList.Parent as FrameworkElement);
        }

        private void CreateNew_ButtonClick(object sender, MouseButtonEventArgs e){
            // change view to function creation
		}

        private void Window_CancelClick(object sender, EventArgs args) {
            Navigator.NavigateBack();
        }


        public void DialArea_ConfirmClick(object sender, EventArgs args) => DialAreaControl.DialArea_ConfirmClick(sender, args);
    }

    public class ListAreaController {
        public void LoadData() {

        }
    }
}
