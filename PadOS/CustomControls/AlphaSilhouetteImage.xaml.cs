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
			"NormalColor", typeof(Brush), typeof(AlphaSilhouetteImage), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x7a, 0xf1))));

		public Brush NormalColor {
			get { return (Brush)GetValue(NormalColorProperty); }
			set { SetValue(NormalColorProperty, value); }
		}


		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
			"Source", typeof(Brush), typeof(AlphaSilhouetteImage), new PropertyMetadata(default(Brush)));

		public Brush Source {
			get { return (Brush)GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}

		public static readonly DependencyProperty ActiveStateProperty = DependencyProperty.Register(
			"ActiveState", typeof(bool), typeof(AlphaSilhouetteImage), new PropertyMetadata(default(bool)));

		public bool ActiveState {
			get { return (bool)GetValue(ActiveStateProperty); }
			set { SetValue(ActiveStateProperty, value); }
		}
	}
}
