namespace PadOS.Views.Settings{
	public partial class NavItem{
		public NavItem(){
			InitializeComponent();
		}

		public static readonly System.Windows.DependencyProperty ImageSourceProperty = System.Windows.DependencyProperty.Register(
			"ImageSource", typeof (System.Windows.Media.ImageSource), typeof (NavItem), new System.Windows.PropertyMetadata(default(System.Windows.Media.ImageSource)));

		public System.Windows.Media.ImageSource ImageSource
		{
			get { return (System.Windows.Media.ImageSource) GetValue(ImageSourceProperty); }
			set { SetValue(ImageSourceProperty, value); }
		}

		public static readonly System.Windows.DependencyProperty IsActiveProperty = System.Windows.DependencyProperty.Register(
			"IsActive", typeof (bool), typeof (NavItem), new System.Windows.PropertyMetadata(default(bool)));

		public bool IsActive
		{
			get { return (bool) GetValue(IsActiveProperty); }
			set { SetValue(IsActiveProperty, value); }
		}
	}
}