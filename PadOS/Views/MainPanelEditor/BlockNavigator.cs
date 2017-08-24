using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace PadOS.Views.MainPanelEditor{
	public class BlockNavigator{

		public static readonly DependencyProperty StartFocusProperty = DependencyProperty.RegisterAttached(
			"StartFocus", typeof(bool), typeof(BlockNavigator), new PropertyMetadata(default(bool)));
		public static bool GetStartFocus(DependencyObject element) => (bool)element.GetValue(StartFocusProperty);
		public static void SetStartFocus(DependencyObject element, bool value) => element.SetValue(StartFocusProperty, value);

		private static readonly DependencyProperty BlockNavigatorProperty = DependencyProperty.RegisterAttached(
			"BlockNavigator", typeof(BlockNavigator), typeof(BlockNavigator), new FrameworkPropertyMetadata(default(BlockNavigator)));
		private static BlockNavigator GetBlockNavigator(UIElement element) => (BlockNavigator)element.GetValue(BlockNavigatorProperty);
		private static void SetBlockNavigator(UIElement element, BlockNavigator value = null) => element.SetValue(BlockNavigatorProperty, value ?? new BlockNavigator(element));

		public static readonly DependencyProperty IsBlockOwnerProperty = DependencyProperty.RegisterAttached(
			"IsBlockOwner", typeof(bool), typeof(BlockNavigator), new FrameworkPropertyMetadata(PropertyChangedCallback));
		public static bool GetIsBlockOwner(UIElement element) => throw new NotImplementedException();
		public static void SetIsBlockOwner(UIElement element, bool value) => element.SetValue(IsBlockOwnerProperty, value);
		private static void PropertyChangedCallback(DependencyObject dep, DependencyPropertyChangedEventArgs args){
			SetBlockNavigator((UIElement) dep);
		}

		public static readonly DependencyProperty RegisteredProperty = DependencyProperty.RegisterAttached(
			"Registered", typeof(bool), typeof(BlockNavigator), new FrameworkPropertyMetadata(RegisteredChanged));
		public static bool GetRegistered(UIElement element) => throw new NotImplementedException();
		public static void SetRegistered(UIElement element, bool value) => element.SetValue(RegisteredProperty, value);
		private static void RegisteredChanged(DependencyObject dep, DependencyPropertyChangedEventArgs args){
			var frameworkElement = dep as FrameworkElement;
			if (frameworkElement == null) return;

			var parent = FindNavigatorElement(frameworkElement);
			var nav = GetBlockNavigator(parent);
			if (nav == null) return;

			frameworkElement.Loaded += delegate{
				nav.AddBlock(frameworkElement);
			};
		}

		private BlockNavigator(UIElement element) {
			var frameworkElement = (FrameworkElement)element;
			if (frameworkElement == null) return;
			frameworkElement.Loaded += delegate{
				var layer = GetAdornerLayer(element);
				var child = GetAdornerContainer(element);
				_cursor = new Cursor((UIElement) child);
				layer.Add(_cursor);
			};
		}

		private FrameworkElement _focusElm;
		private Cursor _cursor;
		private bool _explicitFocusFound;
		private readonly Dictionary<FrameworkElement, Rect> _blocks = new Dictionary<FrameworkElement, Rect>();

		private static FrameworkElement FindNavigatorElement(FrameworkElement elm) {
			var searchElm = elm;
			while (searchElm.Parent != null && (bool)searchElm.GetValue(IsBlockOwnerProperty) == false)
				searchElm = (FrameworkElement)searchElm.Parent;
			return searchElm;
		}

		public void AddBlock(FrameworkElement elm){
			var point = elm
				.TransformToAncestor(FindNavigatorElement(elm))
				.Transform(new Point(0, 0));
			_blocks.Add(
				elm,
				new Rect(
					point.X,
					point.Y,
					elm.ActualWidth,
					elm.ActualHeight
				)
			);
			if (_focusElm == null)
				AddFirstFocus();

			if(_explicitFocusFound == false && _focusElm == null) {
				_focusElm = _blocks.First().Key;
				_cursor.TargetRect = _blocks.First().Value;
			}
		}

		public void AddFirstFocus(){
			foreach (var block in _blocks) {
				if (GetStartFocus(block.Key) == false)
					continue;
				_focusElm = block.Key;
				_explicitFocusFound = true;
				_cursor.TargetRect = block.Value;
				return;
			}
		}

		private static AdornerLayer GetAdornerLayer(Visual visual) {
			var decorator = visual as AdornerDecorator;
			if (decorator != null)
				return decorator.AdornerLayer;
			var presenter = visual as ScrollContentPresenter;
			if (presenter != null)
				return presenter.AdornerLayer;
			var window = visual as Window;
			var visualContent = window?.Content as Visual;
			return AdornerLayer.GetAdornerLayer(visualContent ?? visual);
		}

		private static Visual GetAdornerContainer(Visual visual) {
			var decorator = visual as AdornerDecorator;
			if (decorator != null)
				return decorator.AdornerLayer;
			var presenter = visual as ScrollContentPresenter;
			if (presenter != null)
				return presenter.AdornerLayer;
			var window = visual as Window;
			var visualContent = window?.Content as Visual;
			return visualContent ?? visual;
		}

		private class Rect{
			public Rect(double x, double y, double w, double h){
				Position.X = x;
				Position.Y = y;
				Size.X = w;
				Size.Y = h;
			}

			public Vector2 Position;
			public Vector2 Size;

			public static implicit operator System.Windows.Rect(Rect p){
				return new System.Windows.Rect(p.Position.X, p.Position.Y, p.Size.X, p.Size.Y);
			}
		}
	}
}