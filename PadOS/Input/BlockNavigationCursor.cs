using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PadOS.Input {
	public class BlockNavigationCursor : Adorner {
		public BlockNavigationCursor(UIElement adornedElement) : base(adornedElement) {
		}

		public static readonly DependencyProperty TargetRectProperty = DependencyProperty.Register(
			"TargetRect", typeof(Rect), typeof(BlockNavigationCursor), new PropertyMetadata(default(Rect)));

		public Rect TargetRect
		{
			get => (Rect) GetValue(TargetRectProperty);
			set {
				var rectAnim = new RectAnimation {
					From = _currentRect,
					To = value,
					Duration = new Duration(TimeSpan.FromSeconds(0.1)),
					AutoReverse = false,
				};

				var myStoryboard = new Storyboard();
				myStoryboard.Children.Add(rectAnim);
				myStoryboard.CurrentTimeInvalidated += delegate{
					InvalidateVisual();
				};
				Storyboard.SetTarget(myStoryboard, this);
				Storyboard.SetTargetProperty(rectAnim, new PropertyPath(TargetRectProperty));

				myStoryboard.Begin(this);
				SetValue(TargetRectProperty, value);
				_currentRect = value;
			}
		}

		private Rect _currentRect;

		private readonly SolidColorBrush _fillBrush = new SolidColorBrush(Color.FromArgb(0x7f, 0x99, 0xdd, 0xff));
		private readonly SolidColorBrush _borderBrush = new SolidColorBrush(Color.FromArgb(179, 76, 195, 255));

		protected override void OnRender(DrawingContext ctx) {
			var rect = TargetRect;
			ctx.DrawRectangle(_fillBrush, new Pen(_borderBrush, 2), rect);
		}

		// <Border BorderBrush = "#a9df"
		// BorderThickness="2"
		// Background="#66af" />
	}
}
