using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PadOS.Views.Settings.Controls{
	public partial class BasicListItem {
		public BasicListItem(){
			InitializeComponent();
		}

		public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
			"IsActive", typeof(bool), typeof(BasicListItem), new PropertyMetadata(default(bool)));

		public bool IsActive
		{
			get => (bool)GetValue(IsActiveProperty);
			set => SetValue(IsActiveProperty, value);
		}

		public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
			"ImageSource", typeof(ImageSource), typeof(BasicListItem), new PropertyMetadata(default(ImageSource)));

		public ImageSource ImageSource
		{
			get => (ImageSource)GetValue(ImageSourceProperty);
			set => SetValue(ImageSourceProperty, value);
		}

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			"Text", typeof(string), typeof(BasicListItem), new PropertyMetadata("null"));

		public string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		public ICommand Click { get; set; }

        protected override void OnMouseUp(MouseButtonEventArgs e) {
			Click?.Execute(this);
        }
	}
}