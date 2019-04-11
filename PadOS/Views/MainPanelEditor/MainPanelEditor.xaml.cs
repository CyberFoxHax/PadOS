using System;
using System.ComponentModel;
using System.Windows.Input;

namespace PadOS.Views.MainPanelEditor {
	public partial class MainPanelEditor{
		public MainPanelEditor() {
			InitializeComponent();

			if (DesignerProperties.GetIsInDesignMode(this))
				return;
		}

		private void CreateNew_ButtonClick(object sender, MouseButtonEventArgs e){

		}

		private void OnClick(object sender, EventArgs args){
            var comboBox = sender as ComboBox;
            if (comboBox!=null)
                comboBox.IsOpen = true;
		}
	}
}
