using System.Windows;
using System.Windows.Media;

namespace PadOS.Views.GamePadOSK.Controls {
	public partial class ImageWithLabel {
		public ImageWithLabel() {
			InitializeComponent();
		}

		public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
			"ImageSource", typeof (ImageSource), typeof (ImageWithLabel), new PropertyMetadata(default(ImageSource)));
		public ImageSource ImageSource {
			get => (ImageSource) GetValue(ImageSourceProperty);
			set => SetValue(ImageSourceProperty, value);
		}

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			"Text", typeof (string), typeof (ImageWithLabel), new PropertyMetadata(default(string)));
		public string Text {
			get => (string) GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}
	}
}
