namespace PadOS.Views.Settings.Controls{
	public partial class NavigationListItem : INavigatable{
		public NavigationListItem(){
			InitializeComponent();
		}

		public System.Windows.Input.ICommand Click { get; set; }

		void INavigatable.OnClick() {
			if (Click != null)
				Click.Execute(this);
		}

		public static readonly System.Windows.DependencyProperty IsActiveProperty = System.Windows.DependencyProperty.Register(
			"IsActive", typeof (bool), typeof (NavigationListItem), new System.Windows.PropertyMetadata(default(bool)));

		public bool IsActive
		{
			get { return (bool) GetValue(IsActiveProperty); }
			set { SetValue(IsActiveProperty, value); }
		}

		public static readonly System.Windows.DependencyProperty ImageSourceProperty = System.Windows.DependencyProperty.Register(
			"ImageSource", typeof (System.Windows.Media.ImageSource), typeof (NavigationListItem), new System.Windows.PropertyMetadata(default(System.Windows.Media.ImageSource)));

		public System.Windows.Media.ImageSource ImageSource
		{
			get { return (System.Windows.Media.ImageSource) GetValue(ImageSourceProperty); }
			set { SetValue(ImageSourceProperty, value); }
		}

		public static readonly System.Windows.DependencyProperty TextProperty = System.Windows.DependencyProperty.Register(
			"Text", typeof (string), typeof (NavigationListItem), new System.Windows.PropertyMetadata("<Empty>"));

		public string Text
		{
			get { return (string) GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}
	}
}