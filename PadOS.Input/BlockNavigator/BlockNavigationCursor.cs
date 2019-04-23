using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

// HighlightColor = #66af
// BorderColor = #a9df
// BorderThickness = 2

namespace PadOS.Input.BlockNavigator {
	internal class BlockNavigationCursor : Adorner {
		public BlockNavigationCursor(UIElement adornedElement) : base(adornedElement) {
		}

        private const double BorderThickness = 2;
        private static readonly Color FillColor = Color.FromArgb(0x66, 0x66, 0xaa, 0xff); // #66af
        private static readonly Color BorderColor = Color.FromArgb(0xaa, 0x99, 0xdd, 0xff); // #a9df

        private static readonly DependencyProperty TargetRectProperty = DependencyProperty.Register(
			"TargetRect", typeof(Rect), typeof(BlockNavigationCursor),
            new FrameworkPropertyMetadata(default(Rect), FrameworkPropertyMetadataOptions.AffectsRender)
        );

        private Rect TargetRect {
            get => (Rect)GetValue(TargetRectProperty);
            set => SetValue(TargetRectProperty, value);
        }

        public void SetFocus(Rect rect, bool animate = true) {
            if (RectEquals(TargetRect, rect))
                return;
            if (animate) {
                var rectAnim = new RectAnimation {
                    From = TargetRect,
                    To = rect,
                    Duration = new Duration(TimeSpan.FromSeconds(0.1)),
                    AutoReverse = false,
                };

                var myStoryboard = new Storyboard();
                myStoryboard.Children.Add(rectAnim);
                Storyboard.SetTarget(myStoryboard, this);
                Storyboard.SetTargetProperty(rectAnim, new PropertyPath(TargetRectProperty));
                myStoryboard.Begin(this);
            }

            TargetRect = rect;

            // for no reason at all there will be times when i can update the value
            // but the storyboard function works fine regardless.
            // This is by no means optimal, but i just wanna get on with the day.
            if (animate == false && RectEquals(TargetRect, rect) == false) {
                var rectAnim = new RectAnimation {
                    From = TargetRect,
                    To = rect,
                    Duration = new Duration(TimeSpan.FromSeconds(0.0)),
                    AutoReverse = false,
                };

                var myStoryboard = new Storyboard();
                myStoryboard.Children.Add(rectAnim);
                Storyboard.SetTarget(myStoryboard, this);
                Storyboard.SetTargetProperty(rectAnim, new PropertyPath(TargetRectProperty));
                myStoryboard.Begin(this);
            }
        }

        private static bool RectEquals(Rect a, Rect b) {
            const double Threshold = 0.01;
            return
                Math.Abs(a.Left   - b.Left) < Threshold && 
                Math.Abs(a.Top    - b.Top) < Threshold && 
                Math.Abs(a.Width  - b.Width) < Threshold &&
                Math.Abs(a.Height - b.Height) < Threshold;
        }

		private static readonly SolidColorBrush _fillBrush = new SolidColorBrush(Color.FromArgb(0x7f, 0x99, 0xdd, 0xff));
		private static readonly SolidColorBrush _borderBrush = new SolidColorBrush(Color.FromArgb(179, 76, 195, 255));

		protected override void OnRender(DrawingContext ctx) {
			var rect = TargetRect;
			ctx.DrawRectangle(_fillBrush, new Pen(_borderBrush, 2), rect);
		}

	}
}
