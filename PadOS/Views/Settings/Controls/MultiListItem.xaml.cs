using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using PadOS.Commands;

namespace PadOS.Views.Settings.Controls{
	
	public partial class MultiListItem : INavigatable, Input.IGamePadFocusable{
		public MultiListItem(){
			InitializeComponent();
			Items = new System.Collections.Generic.List<MultiListItemSubItem>();
		}

		void INavigatable.OnClick() {
			Dispatcher.BeginInvoke(new System.Action(() => {
				var activeItem = Items.First(p => p.IsActive);
				((INavigatable)activeItem).OnClick();
				if (Click != null) Click.Execute(this);
			}));
		}

		public System.Windows.Input.ICommand Click { get; set; }

		private MultiListItemSubItem _defaultFocusElement;
		public bool IsGamePadFocused { get; set; }

		public static readonly System.Windows.DependencyProperty IsActiveProperty = System.Windows.DependencyProperty.Register(
			"IsActive", typeof(bool), typeof(MultiListItem), new System.Windows.PropertyMetadata(default(bool), IsActiveChanged));

		private static void IsActiveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs){
			var appName = WindowCommands.GetApplicationName;
			((MultiListItem)dependencyObject).Text = appName;
		}

		public bool IsActive {
			get { return (bool)GetValue(IsActiveProperty); }
			set
			{
				if (_defaultFocusElement == null)
					_defaultFocusElement = Items.FirstOrDefault(p => p.IsActive) ?? Items.First();
				SetValue(IsActiveProperty, value);
				IsGamePadFocused = value;

				if (value){
					if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)) return;
					Input.WpfGamepad.XInput.ThumbLeftChange += XInputOnThumbLeftChange;
					Input.WpfGamepad.XInput.DPadRightDown	+= XInputOnDPadRightDown;
					Input.WpfGamepad.XInput.DPadLeftDown	+= XInputOnDPadLeftDown;
					_defaultFocusElement.IsActive = true;
				}
				else{
					_defaultFocusElement = Items.FirstOrDefault(p => p.IsActive) ?? Items.First();
					foreach (var item in Items)
						item.IsActive = false;
					if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)) return;
					Input.WpfGamepad.XInput.ThumbLeftChange -= XInputOnThumbLeftChange;
					Input.WpfGamepad.XInput.DPadRightDown	-= XInputOnDPadRightDown;
					Input.WpfGamepad.XInput.DPadLeftDown	-= XInputOnDPadLeftDown;
				}
			}
		}

		private bool _thumbstickWaitForReturn;

		private void XInputOnThumbLeftChange(XInputDotNetPure.PlayerIndex player, XInputDotNetPure.GamePadState state, Vector2 value){
			if (System.Math.Abs(value.X) < 0.3){
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

		private void XInputOnDPadLeftDown(XInputDotNetPure.PlayerIndex player, XInputDotNetPure.GamePadState state){
			MovePrevious();
		}

		private void XInputOnDPadRightDown(XInputDotNetPure.PlayerIndex player, XInputDotNetPure.GamePadState state){
			MoveNext();
		}

		private void MoveNext(){
			Dispatcher.BeginInvoke(new System.Action(() => {
				var activeItem = Items.First(p => p.IsActive);
				var index = Items.IndexOf(activeItem);
				var oldItem = activeItem;
				var newItem = Items[(index + 1) % Items.Count];
				oldItem.IsActive = false;
				newItem.IsActive = true;
			}));
		}

		private void MovePrevious(){
			Dispatcher.BeginInvoke(new System.Action(() => {
				var activeItem = Items.First(p => p.IsActive);
				var index = Items.IndexOf(activeItem);
				var oldItem = activeItem;
				var newItem = Items[index <= 0 ? Items.Count - 1 : index - 1];
				oldItem.IsActive = false;
				newItem.IsActive = true;
			}));
		}

		public static readonly System.Windows.DependencyProperty ImageSourceProperty = System.Windows.DependencyProperty.Register(
			"ImageSource", typeof(System.Windows.Media.ImageSource), typeof(MultiListItem), new System.Windows.PropertyMetadata(default(System.Windows.Media.ImageSource)));

		public System.Windows.Media.ImageSource ImageSource {
			get { return (System.Windows.Media.ImageSource)GetValue(ImageSourceProperty); }
			set { SetValue(ImageSourceProperty, value); }
		}

		public static readonly System.Windows.DependencyProperty TextProperty = System.Windows.DependencyProperty.Register(
			"Text", typeof(string), typeof(MultiListItem), new System.Windows.PropertyMetadata("<Empty>"));

		public string Text {
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public static readonly System.Windows.DependencyProperty ItemsProperty = System.Windows.DependencyProperty.Register(
			"Items", typeof(System.Collections.Generic.List<MultiListItemSubItem>), typeof(MultiListItem), new System.Windows.PropertyMetadata(default(System.Collections.Generic.List<MultiListItemSubItem>)));

		public System.Collections.Generic.List<MultiListItemSubItem> Items {
			get { return (System.Collections.Generic.List<MultiListItemSubItem>)GetValue(ItemsProperty); }
			set { SetValue(ItemsProperty, value); }
		}
	}
}