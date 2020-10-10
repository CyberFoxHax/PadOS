using System.Windows;
using System.Windows.Media;

namespace PadOS.CustomControls {
	public partial class AlphaSilhouetteImage {
		public AlphaSilhouetteImage() {
			InitializeComponent();
		}

		public static readonly DependencyProperty ActiveColorProperty = DependencyProperty.Register(
			"ActiveColor", typeof(Brush), typeof(AlphaSilhouetteImage), new PropertyMetadata(Brushes.White));

		public Brush ActiveColor {
			get { return (Brush)GetValue(ActiveColorProperty); }
			set { SetValue(ActiveColorProperty, value); }
		}

		public static readonly DependencyProperty NormalColorProperty = DependencyProperty.Register(
			"NormalColor", typeof(Brush), typeof(AlphaSilhouetteImage), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0xff, 0x4f, 0x4f, 0x4f))));

		public Brush NormalColor {
			get { return (Brush)GetValue(NormalColorProperty); }
			set { SetValue(NormalColorProperty, value); }
		}


		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
			"Source", typeof(ImageSource), typeof(AlphaSilhouetteImage), new PropertyMetadata(default(ImageSource)));

		public ImageSource Source {
			get { return (ImageSource)GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}

		public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
			"IsActive", typeof(bool), typeof(AlphaSilhouetteImage), new PropertyMetadata(default(bool)));

		public bool IsActive {
			get { return (bool)GetValue(IsActiveProperty); }
			set { SetValue(IsActiveProperty, value); }
		}
	}
}
