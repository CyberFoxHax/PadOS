using System;
using System.Windows;
using PadOS.Navigation;
using XInputDotNetPure;

namespace PadOS.Input {
	public partial class WpfGamePad : IDisposable {
		private static readonly GamePadInput XInput = GamePadInput.StaticInputInstance;
		private static readonly RoutedEvent[] ButtonEvents = {
			ButtonADown, ButtonAUp,
			ButtonBDown, ButtonBUp,
			ButtonXDown, ButtonXUp,
			ButtonYDown, ButtonYUp,
			ButtonBackDown, ButtonBackUp,
			ButtonGuideDown, ButtonGuideUp,
			ButtonLeftShoulderDown, ButtonLeftShoulderUp,
			ButtonLeftStickDown, ButtonLeftStickUp,
			ButtonRightShoulderDown, ButtonRightShoulderUp,
			ButtonRightStickDown, ButtonRightStickUp,
			ButtonStartDown, ButtonStartUp,
			DPadLeftDown, DPadLeftUp,
			DPadRightDown, DPadRightUp,
			DPadUpDown, DPadUpUp,
			DPadDownDown, DPadDownUp
		};

		public WpfGamePad(UIElement focusOwner) {
			_focusOwner = focusOwner;
			BlockNavigator.AddCursorEnterHandler(_focusOwner, OnCursorEnter);
			BlockNavigator.AddCursorExitHandler(_focusOwner, OnCursorExit);

			if (_focusOwner is Window window) {
				_focusOwner.IsVisibleChanged += FocusOwnerOnIsVisibleChanged;
				window.Closed += OnWindowClosed;
				AttachEvents();
			}
			DetachEvents();
		}

		private readonly UIElement _focusOwner;
		private bool _eventsIsAttached;

		private void DetachEvents() {
			if (_eventsIsAttached == false) return;
			_eventsIsAttached = false;
			XInput.ThumbLeftChange -= OnXInputOnThumbLeftChange;
		}

		private void AttachEvents(){
			if (_eventsIsAttached) return;
			_eventsIsAttached = true;
			XInput.ThumbLeftChange += OnXInputOnThumbLeftChange;
			//foreach (var routedEvent in ButtonEvents) {
			//	typeof(GamePadInput)
			//		.GetEvent(routedEvent.Name)hh
			//		.AddMethod
			//		.Invoke(XInput, new object[]{
			//			(GamePadEvent)new Forwarder(routedEvent, XInputOnButtonAUp).GamePadEvent
			//		});
			//}
		}

		private void OnXInputOnThumbLeftChange(int player, GamePadState state, Vector2 value){
			_focusOwner.Dispatcher.Invoke(
				() => _focusOwner.RaiseEvent(
					new GamePadEventArgs<Vector2>(ThumbLeftChange, _focusOwner){
						PlayerIndex = player,
						GamePadState = state,
						Value = value
					}
				)
			);
		}

		private void OnCursorExit(object sender, EventArgs args) {
			DetachEvents();
		}

		private void OnCursorEnter(object sender, EventArgs args) {
			AttachEvents();
		}

		private void FocusOwnerOnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args) {
			if ((bool) args.NewValue) {
				AttachEvents();
			}
			else {
				DetachEvents();
			}
		}
		
		private void OnWindowClosed(object sender, EventArgs eventArgs) {
			Dispose();
		}

		public void Dispose(){
			DetachEvents();

			_focusOwner.IsVisibleChanged -= FocusOwnerOnIsVisibleChanged;
			BlockNavigator.RemoveCursorExitHandler(_focusOwner, OnCursorExit);
			BlockNavigator.RemoveCursorEnterHandler(_focusOwner, OnCursorEnter);

			if (_focusOwner is Window window)
				window.Closed -= OnWindowClosed;
		}
	}

}
