using PadOS.Navigation;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace PadOS.Views.MainPanelEditor {
    public partial class MainPanelEditor{
		public MainPanelEditor() {
			InitializeComponent();
            DialAreaController = new DialAreaController(DialArea_ButtonsCanvas, HighlightRotate, Highlight);
            var ctx = new SaveData.SaveData();
            Profiles_ComboBox.ItemsSource = ctx.Profiles.Select(p=>p.Name).ToList();


            DialAreaController.LoadPanel(ctx.Profiles.First());
            

            if (DesignerProperties.GetIsInDesignMode(this))
				return;
		}


		private void CreateNew_ButtonClick(object sender, MouseButtonEventArgs e){

		}

        private void Window_CancelClick(object sender, EventArgs args) {
            Navigator.NavigateBack();
        }

        private DialAreaController DialAreaController;
        public void DialArea_ConfirmClick(object sender, EventArgs args) => DialAreaController.DialArea_ConfirmClick(sender, args);
    }
}
