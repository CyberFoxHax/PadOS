namespace PadOS.Views.Settings.Controls{
	public partial class MultiListItemSubItem {
		public MultiListItemSubItem(){
			InitializeComponent();
		}

		public System.Windows.Input.ICommand Click { get; set; }

		public void OnClick(){
			Click?.Execute(this);
		}

		public static readonly System.Windows.DependencyProperty ImageSourceProperty = System.Windows.DependencyProperty.Register(
			"ImageSource", typeof (System.Windows.Media.ImageSource), typeof (MultiListItemSubItem), new System.Windows.PropertyMetadata(default(System.Windows.Media.ImageSource)));

		public System.Windows.Media.ImageSource ImageSource
		{
			get => (System.Windows.Media.ImageSource) GetValue(ImageSourceProperty);
			set => SetValue(ImageSourceProperty, value);
		}

		public static readonly System.Windows.DependencyProperty IsActiveProperty = System.Windows.DependencyProperty.Register(
			"IsActive", typeof (bool), typeof (MultiListItemSubItem), new System.Windows.PropertyMetadata(default(bool)));

		public bool IsActive
		{
			get => (bool) GetValue(IsActiveProperty);
			set => SetValue(IsActiveProperty, value);
		}
	}
}