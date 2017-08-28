using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using PadOS.Input;
using PadOS.Views.MainPanelEditor;
using XInputDotNetPure;

namespace PadOS.Navigation{
	public partial class BlockNavigator{
		private BlockNavigator(UIElement element) {
			_xInput = new GamePadInput();
			InitGamepad();
			var frameworkElement = (FrameworkElement)element;
			if (frameworkElement == null) return;
			_ownerElement = frameworkElement;
			frameworkElement.Loaded += delegate{
				var layer = GetAdornerLayer(element);
				var child = (element as Window)?.Content as Visual ?? element;
				_cursor = new Cursor((UIElement) child);
				layer.Add(_cursor);
			};
		}

		private FrameworkElement _focusElm;
		private Cursor _cursor;
		private bool _explicitFocusFound;
		private readonly Dictionary<FrameworkElement, Rect> _blocks = new Dictionary<FrameworkElement, Rect>();
		private readonly FrameworkElement _ownerElement;

		private void SetFocus(FrameworkElement elm){
			_focusElm.Dispatcher.Invoke(() => { 
				_focusElm?.RaiseEvent(new RoutedEventArgs(CursorExitEvent, _focusElm));
				_focusElm = elm;
				_focusElm.RaiseEvent(new RoutedEventArgs(CursorEnterEvent, _focusElm));
				_cursor.TargetRect = _blocks[_focusElm];
			});
		}

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

		private void AddFirstFocus(){
			foreach (var block in _blocks) {
				if (GetInitialFocus(block.Key) == false)
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

		private static void RegisteredChanged(DependencyObject dep, DependencyPropertyChangedEventArgs args) {
			var frameworkElement = dep as FrameworkElement;
			if (frameworkElement == null) return;

			var parent = FindNavigatorElement(frameworkElement);
			var nav = GetBlockNavigator(parent);
			if (nav == null) return;

			frameworkElement.Loaded += delegate {
				nav.AddBlock(frameworkElement);
			};
		}
	}
}