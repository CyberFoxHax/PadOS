using System.Windows;

namespace PadOS.Views.Settings.Controls{
	public partial class BasicListItem {
		public BasicListItem(){
			InitializeComponent();
		}

		public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
			"IsActive", typeof(bool), typeof(BasicListItem), new PropertyMetadata(default(bool)));

		public bool IsActive {
			get => (bool) GetValue(IsActiveProperty);
			set => SetValue(IsActiveProperty, value);
		}

		public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
			"ImageSource", typeof(System.Windows.Media.ImageSource), typeof(BasicListItem), new PropertyMetadata(default(System.Windows.Media.ImageSource)));

		public System.Windows.Media.ImageSource ImageSource {
			get => (System.Windows.Media.ImageSource)GetValue(ImageSourceProperty);
			set => SetValue(ImageSourceProperty, value);
		}

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			"Text", typeof(string), typeof(BasicListItem), new PropertyMetadata("null"));

		public string Text {
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		public System.Windows.Input.ICommand Click { get; set; }

		public virtual void OnClick() {
			Click?.Execute(this);
		}
	}
}