using System.Windows;
using XInputDotNetPure;

namespace PadOS.Input{
	public class GamePadEventArgs : RoutedEventArgs {
		public GamePadEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) {}

		public int PlayerIndex { get; set; }
		public GamePadState GamePadState { get; set; }
	}

	public class GamePadEventArgs<TValue> : GamePadEventArgs {
		public GamePadEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) {}

		public TValue Value { get; set; }
	}
}