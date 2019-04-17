using System;
using System.Collections.Generic;
using System.Windows;
using XInputDotNetPure;

namespace PadOS.Input.WpfGamePad {
	public partial class WpfGamePad : IDisposable {
		private static readonly GamePadInput.GamePadInput XInput = GamePadInput.GamePadInput.StaticInputInstance;
		public WpfGamePad(UIElement focusOwner) {
			_focusOwner = focusOwner;

			if (_focusOwner is Window window) {
				_focusOwner.IsVisibleChanged += FocusOwnerOnIsVisibleChanged;
				window.Closed += OnWindowClosed;
				AttachEvents();
			}
			DetachEvents();
		}

		private readonly UIElement _focusOwner;
		private bool _eventsIsAttached;
		private readonly Dictionary<RoutedEvent, GamePadEvent> _buttonEvents = new Dictionary<RoutedEvent, GamePadEvent>();
		private readonly Dictionary<RoutedEvent, GamePadEvent<Vector2>> _thumbstickEvents = new Dictionary<RoutedEvent, GamePadEvent<Vector2>>();
		private readonly Dictionary<RoutedEvent, GamePadEvent<float>> _triggerEvents = new Dictionary<RoutedEvent, GamePadEvent<float>>();

        private void FocusChanged(bool isFocused) {
            if (isFocused)
                AttachEvents();
            else
                DetachEvents();
        }

        private void DetachEvents() {
			if (_eventsIsAttached == false) return;

			foreach (var routedEvent in ButtonEvents){
				var dict = _buttonEvents;
				RemoveXInputHandler(routedEvent, dict[routedEvent]);
				dict.Remove(routedEvent);
			}

			foreach (var routedEvent in ThumbstickEvents) {
				var dict = _thumbstickEvents;
				RemoveXInputHandler(routedEvent, dict[routedEvent]);
				dict.Remove(routedEvent);
			}

			foreach (var routedEvent in TriggerEvents) {
				var dict = _triggerEvents;
				RemoveXInputHandler(routedEvent, dict[routedEvent]);
				dict.Remove(routedEvent);
			}

			_eventsIsAttached = false;
		}

		private void AttachEvents(){
			if (_eventsIsAttached) return;

			foreach (var routedEvent in ButtonEvents){
				var dict = _buttonEvents;
				void OnEvent(int a, GamePadState b) => OnButton(routedEvent, a, b);
				dict.Add(routedEvent, OnEvent);
				AddXInputEvent(routedEvent, (GamePadEvent)OnEvent);
			}

			foreach (var routedEvent in ThumbstickEvents){
				var dict = _thumbstickEvents;
				void OnEvent(int a, GamePadState b, Vector2 c) => OnAnalogueChange(routedEvent, a, b, c);
				dict.Add(routedEvent, OnEvent);
				AddXInputEvent(routedEvent, (GamePadEvent<Vector2>)OnEvent);
			}

			foreach (var routedEvent in TriggerEvents){
				var dict = _triggerEvents;
				void OnEvent(int a, GamePadState b, float c) => OnAnalogueChange(routedEvent, a, b, c);
				dict.Add(routedEvent, OnEvent);
				AddXInputEvent(routedEvent, (GamePadEvent<float>)OnEvent);
			}

			_eventsIsAttached = true;
		}

		private void RemoveXInputHandler(RoutedEvent routedEvent, Delegate handler) {
			typeof(GamePadInput.GamePadInput)
				.GetEvent(routedEvent.Name)
				.RemoveMethod
				.Invoke(XInput, new object[] { handler });
		}

		private void AddXInputEvent(RoutedEvent routedEvent, Delegate handler){
			typeof(GamePadInput.GamePadInput)
				.GetEvent(routedEvent.Name)
				.AddMethod
				.Invoke(XInput, new object[]{ handler });
		}

		//////////////////////////// <Handlers> ////////////////////////
		private void OnButton(RoutedEvent routedEvent, int player, GamePadState state) {
			_focusOwner.Dispatcher.Invoke(
				() => _focusOwner.RaiseEvent(
					new GamePadEventArgs(routedEvent, _focusOwner) {
						PlayerIndex = player,
						GamePadState = state,
					}
				)
			);
		}

		private void OnAnalogueChange<T>(RoutedEvent routedEvent, int player, GamePadState state, T value) {
			_focusOwner.Dispatcher.Invoke(
				() => _focusOwner.RaiseEvent(
					new GamePadEventArgs<T>(routedEvent, _focusOwner) {
						PlayerIndex = player,
						GamePadState = state,
						Value = value
					}
				)
			);
		}
		//////////////////////////// </Handlers> ///////////////////////

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

			if (_focusOwner is Window window)
				window.Closed -= OnWindowClosed;
		}
	}

}
