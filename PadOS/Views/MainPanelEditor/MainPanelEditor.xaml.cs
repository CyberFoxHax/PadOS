using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

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
			throw new NotImplementedException();
		}
	}
}
