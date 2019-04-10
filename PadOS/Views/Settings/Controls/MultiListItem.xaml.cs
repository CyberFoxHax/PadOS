using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using PadOS.Commands;
using PadOS.Input;

namespace PadOS.Views.Settings.Controls{
	
	public partial class MultiListItem : IDisposable{
		public MultiListItem(){
			InitializeComponent();
			Items = new Collection<MultiListItemSubItem>();
			Loaded+= OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs){
			if (_defaultFocusElement == null)
				_defaultFocusElement = Items.FirstOrDefault(p => p.IsActive) ?? Items.First();
		}

		public System.Windows.Input.ICommand Click { get; set; }

		private MultiListItemSubItem ActiveItem => Items.First(p => p.IsActive);
		private MultiListItemSubItem _defaultFocusElement;

		public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
			"IsActive", typeof(bool), typeof(MultiListItem), new PropertyMetadata(default(bool), IsActiveChanged));

		private static void IsActiveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args){
			var appName = WindowCommands.ApplicationName;
			((MultiListItem)dependencyObject).Text = appName;
		}

		public bool IsActive {
			get => (bool)GetValue(IsActiveProperty);
			set => SetValue(IsActiveProperty, value);
		}

		private bool _thumbstickWaitForReturn;


		private void MultiListItem_OnConfirmClick(object sender, EventArgs args) {
			ActiveItem.OnClick();
			Click?.Execute(this);
		}

		private void MultiListItem_OnCursorExit(object sender, EventArgs args) {
			var oldItem = ActiveItem;
			var newItem = _defaultFocusElement;
			oldItem.IsActive = false;
			newItem.IsActive = true;
		}

		private void XInputOnThumbLeftChange(object sender, GamePadEventArgs<Vector2> args){
			var value = args.Value;
			if (Math.Abs(value.X) < 0.3){
				_thumbstickWaitForReturn = false;
				return;
			}
			if(_thumbstickWaitForReturn)
				return;

			_thumbstickWaitForReturn = true;

			if (value.X > 0)
				MoveNext();
			else if (value.X < 0)
				MovePrevious();
		}

		private void XInputOnDPadLeftDown(object sender, GamePadEventArgs args){
			MovePrevious();
		}

		private void XInputOnDPadRightDown(object sender, GamePadEventArgs args){
			MoveNext();
		}

		private void MoveNext(){
			var activeItem = Items.First(p => p.IsActive);
			var index = Items.IndexOf(activeItem);
			var oldItem = activeItem;
			var newItem = Items[(index + 1) % Items.Count];
			oldItem.IsActive = false;
			newItem.IsActive = true;
		}

		private void MovePrevious(){
			var activeItem = Items.First(p => p.IsActive);
			var index = Items.IndexOf(activeItem);
			var oldItem = activeItem;
			var newItem = Items[index <= 0 ? Items.Count - 1 : index - 1];
			oldItem.IsActive = false;
			newItem.IsActive = true;
		}

		public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
			"ImageSource", typeof(System.Windows.Media.ImageSource), typeof(MultiListItem), new PropertyMetadata(default(System.Windows.Media.ImageSource)));

		public System.Windows.Media.ImageSource ImageSource {
			get => (System.Windows.Media.ImageSource)GetValue(ImageSourceProperty);
			set => SetValue(ImageSourceProperty, value);
		}

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			"Text", typeof(string), typeof(MultiListItem), new PropertyMetadata("<Empty>"));

		public string Text {
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
			"Items", typeof(Collection<MultiListItemSubItem>), typeof(MultiListItem), new PropertyMetadata(null));

		public Collection<MultiListItemSubItem> Items {
			get => (Collection<MultiListItemSubItem>) GetValue(ItemsProperty);
			set => SetValue(ItemsProperty, value);
		}

		public void Dispose(){
			IsActive = false;
		}
	}
}