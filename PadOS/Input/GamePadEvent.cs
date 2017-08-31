using XInputDotNetPure;

namespace PadOS.Input{
	public delegate void GamePadEvent(int player, GamePadState state);
	public delegate void GamePadEvent<in T>(int player, GamePadState state, T value);
}