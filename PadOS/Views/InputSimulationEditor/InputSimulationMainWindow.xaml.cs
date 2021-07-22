using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PadOS.Views.InputSimulationEditor
{
    public partial class InputSimulationMainWindow : Window {
        public InputSimulationMainWindow() {
            InitializeComponent();
        }

        private void Window_CancelClick(object sender, EventArgs args) {
            PadOS.Navigation.Navigator.NavigateBack();
        }
    }
}
