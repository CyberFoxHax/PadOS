namespace PadOS.Views.Settings.Controls{
	public partial class BasicListItem {
		public BasicListItem(){
			InitializeComponent();
		}

		public System.Windows.Input.ICommand Click { get; set; }

		public void OnClick(){
			Click?.Execute(this);
		}

		public static readonly System.Windows.DependencyProperty IsActiveProperty = System.Windows.DependencyProperty.Register(
			"IsActive", typeof(bool), typeof(BasicListItem), new System.Windows.PropertyMetadata(default(bool)));

		public bool IsActive {
			get => (bool)GetValue(IsActiveProperty);
			set => SetValue(IsActiveProperty, value);
		}

		public static readonly System.Windows.DependencyProperty ImageSourceProperty = System.Windows.DependencyProperty.Register(
			"ImageSource", typeof(System.Windows.Media.ImageSource), typeof(BasicListItem), new System.Windows.PropertyMetadata(default(System.Windows.Media.ImageSource)));

		public System.Windows.Media.ImageSource ImageSource {
			get => (System.Windows.Media.ImageSource)GetValue(ImageSourceProperty);
			set => SetValue(ImageSourceProperty, value);
		}

		public static readonly System.Windows.DependencyProperty TextProperty = System.Windows.DependencyProperty.Register(
			"Text", typeof(string), typeof(BasicListItem), new System.Windows.PropertyMetadata("<Empty>"));

		public string Text {
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}
	}
}