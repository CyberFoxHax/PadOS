namespace PadOS.Views.Settings{
	public partial class BasicListItem : INavigatable {
		public BasicListItem(){
			InitializeComponent();
		}

		public event System.Action Activate;

		void INavigatable.Activate(){
			if (Activate != null)
				Activate();
		}

		public static readonly System.Windows.DependencyProperty IsActiveProperty = System.Windows.DependencyProperty.Register(
			"IsActive", typeof(bool), typeof(BasicListItem), new System.Windows.PropertyMetadata(default(bool)));

		public bool IsActive {
			get { return (bool)GetValue(IsActiveProperty); }
			set { SetValue(IsActiveProperty, value); }
		}

		public static readonly System.Windows.DependencyProperty ImageSourceProperty = System.Windows.DependencyProperty.Register(
			"ImageSource", typeof(System.Windows.Media.ImageSource), typeof(BasicListItem), new System.Windows.PropertyMetadata(default(System.Windows.Media.ImageSource)));

		public System.Windows.Media.ImageSource ImageSource {
			get { return (System.Windows.Media.ImageSource)GetValue(ImageSourceProperty); }
			set { SetValue(ImageSourceProperty, value); }
		}

		public static readonly System.Windows.DependencyProperty TextProperty = System.Windows.DependencyProperty.Register(
			"Text", typeof(string), typeof(BasicListItem), new System.Windows.PropertyMetadata("<Empty>"));

		public string Text {
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}
	}
}