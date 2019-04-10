using System.Windows;
using PadOS.Views.Settings.Controls;

namespace PadOS.Views.Settings {
	public partial class Settings {
		public Settings(){
			InitializeComponent();

			foreach (FrameworkElement child in ButtonsList.Children){
				if (child is BasicListItem basicListItem)
					basicListItem.ClearValue(BasicListItem.IsActiveProperty);
				if (child is NavigationListItem navItem)
					navItem.ClearValue(NavigationListItem.IsActiveProperty);
				if (child is MultiListItem multiItem)
					multiItem.ClearValue(MultiListItem.IsActiveProperty);
			}

        }
    }
}
