using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace PadOS.Views.MainPanelEditor {
	public partial class ComboBox {
		public ComboBox() {
			InitializeComponent();

			if (DesignerProperties.GetIsInDesignMode(this))
				return;
		}

		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
			"ItemsSource", typeof(List<string>), typeof(ComboBox), new PropertyMetadata(default(List<string>)));

		public List<string> ItemsSource
		{
			get => (List<string>) GetValue(ItemsSourceProperty);
			set {
				ListBox.ItemsSource = value;
				SetValue(ItemsSourceProperty, value);
			}
		}

		public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
			"IsOpen", typeof(bool), typeof(ComboBox), new PropertyMetadata(default(bool)));

		public bool IsOpen
		{
			get { return (bool) GetValue(IsOpenProperty); }
			set {
				Popup.IsOpen = value;
				SetValue(IsOpenProperty, value);
			}
		}
	}
}
