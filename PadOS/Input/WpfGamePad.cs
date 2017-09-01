using System;
using System.Collections.Generic;
using System.Windows;
using PadOS.Navigation;
using XInputDotNetPure;

namespace PadOS.Input {
	public partial class WpfGamePad : IDisposable {
		private static readonly GamePadInput XInput = GamePadInput.StaticInputInstance;
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
		private readonly Dictionary<RoutedEvent, GamePadEvent> _buttonEvents = new Dictionary<RoutedEvent, GamePadEvent>();
		private readonly Dictionary<RoutedEvent, GamePadEvent<Vector2>> _thumbstickEvents = new Dictionary<RoutedEvent, GamePadEvent<Vector2>>();
		private readonly Dictionary<RoutedEvent, GamePadEvent<float>> _triggerEvents = new Dictionary<RoutedEvent, GamePadEvent<float>>();

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
			typeof(GamePadInput)
				.GetEvent(routedEvent.Name)
				.RemoveMethod
				.Invoke(XInput, new object[] { handler });
		}

		private void AddXInputEvent(RoutedEvent routedEvent, Delegate handler){
			typeof(GamePadInput)
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
			BlockNavigator.RemoveCursorExitHandler(_focusOwner, OnCursorExit);
			BlockNavigator.RemoveCursorEnterHandler(_focusOwner, OnCursorEnter);

			if (_focusOwner is Window window)
				window.Closed -= OnWindowClosed;
		}
	}

}
