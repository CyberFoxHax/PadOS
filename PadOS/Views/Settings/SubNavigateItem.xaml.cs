namespace PadOS.Views.Settings{
	public partial class SubNavigateItem : INavigatable{
		public SubNavigateItem(){
			InitializeComponent();
		}

		public event System.Action Activate;

		void INavigatable.Activate() {
			if (Activate != null)
				Activate();
		}

		public static readonly System.Windows.DependencyProperty ImageSourceProperty = System.Windows.DependencyProperty.Register(
			"ImageSource", typeof (System.Windows.Media.ImageSource), typeof (SubNavigateItem), new System.Windows.PropertyMetadata(default(System.Windows.Media.ImageSource)));

		public System.Windows.Media.ImageSource ImageSource
		{
			get { return (System.Windows.Media.ImageSource) GetValue(ImageSourceProperty); }
			set { SetValue(ImageSourceProperty, value); }
		}

		public static readonly System.Windows.DependencyProperty IsActiveProperty = System.Windows.DependencyProperty.Register(
			"IsActive", typeof (bool), typeof (SubNavigateItem), new System.Windows.PropertyMetadata(default(bool)));

		public bool IsActive
		{
			get { return (bool) GetValue(IsActiveProperty); }
			set { SetValue(IsActiveProperty, value); }
		}
	}
}