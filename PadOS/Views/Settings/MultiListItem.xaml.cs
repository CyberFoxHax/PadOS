namespace PadOS.Views.Settings{
	
	public partial class MultiListItem : INavigatable{
		public MultiListItem(){
			InitializeComponent();
		}

		public static readonly System.Windows.DependencyProperty IsActiveProperty = System.Windows.DependencyProperty.Register(
			"IsActive", typeof(bool), typeof(MultiListItem), new System.Windows.PropertyMetadata(default(bool)));

		public bool IsActive {
			get { return (bool)GetValue(IsActiveProperty); }
			set { SetValue(IsActiveProperty, value); }
		}

		public static readonly System.Windows.DependencyProperty ImageSourceProperty = System.Windows.DependencyProperty.Register(
			"ImageSource", typeof(System.Windows.Media.ImageSource), typeof(MultiListItem), new System.Windows.PropertyMetadata(default(System.Windows.Media.ImageSource)));

		public System.Windows.Media.ImageSource ImageSource {
			get { return (System.Windows.Media.ImageSource)GetValue(ImageSourceProperty); }
			set { SetValue(ImageSourceProperty, value); }
		}

		public static readonly System.Windows.DependencyProperty TextProperty = System.Windows.DependencyProperty.Register(
			"Text", typeof(string), typeof(MultiListItem), new System.Windows.PropertyMetadata("<Empty>"));

		public string Text {
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public static readonly System.Windows.DependencyProperty ItemsProperty = System.Windows.DependencyProperty.Register(
			"Items", typeof(System.Collections.Generic.List<NavItem>), typeof(MultiListItem), new System.Windows.PropertyMetadata(new System.Collections.Generic.List<NavItem>()));

		public System.Collections.Generic.List<NavItem> Items
		{
			get { return (System.Collections.Generic.List<NavItem>)GetValue(ItemsProperty); }
			set { SetValue(ItemsProperty, value); }
		}	
	}
}