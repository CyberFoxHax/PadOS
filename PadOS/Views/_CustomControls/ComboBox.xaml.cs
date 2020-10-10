using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PadOS.Views.CustomControls {
    public partial class ComboBox {
		public ComboBox() {
			InitializeComponent();

			if (DesignerProperties.GetIsInDesignMode(this))
				return;

        }


        public delegate void ItemClickedEvent(object sender, ComboBoxItemContainer item);
        public event ItemClickedEvent ItemClicked;

        protected override void OnMouseUp(MouseButtonEventArgs e) {
            IsOpen = !IsOpen;
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
			"ItemsSource", typeof(List<ComboBoxItemContainer>), typeof(ComboBox), new PropertyMetadata(default(List<ComboBoxItemContainer>)));

		public List<ComboBoxItemContainer> ItemsSource
		{
			get => (List<ComboBoxItemContainer>) GetValue(ItemsSourceProperty);
			set {
                ListBox.Items.Clear();
                ListBox.ItemsSource = value;
                TextElement.Text = value.FirstOrDefault()?.Text ?? "";
                SetValue(ItemsSourceProperty, value);
			}
		}

		public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
			"IsOpen", typeof(bool), typeof(ComboBox), new PropertyMetadata(default(bool)));

		public bool IsOpen
		{
			get { return (bool) GetValue(IsOpenProperty); }
			set {
                Popup.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
				SetValue(IsOpenProperty, value);
			}
		}

        public void Open() {
            IsOpen = true;
        }

        public void Close() {
            IsOpen = false;
        }

        private void NavigationEnter(object sender, System.EventArgs args) {
            Open();
        }

        private void NavigationExit(object sender, System.EventArgs args) {
            Close();
        }

        private void ListBoxItem_MouseUp(object sender, MouseButtonEventArgs e) {
            e.Handled = true;
            var frameworkElement = (FrameworkElement)sender;
            var data = (ComboBoxItemContainer)frameworkElement.DataContext;
            TextElement.Text = data.Text;
            ItemClicked?.Invoke(sender, data);
        }
    }
}
