﻿using System.ComponentModel;
using System.IO;
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
	}
}
