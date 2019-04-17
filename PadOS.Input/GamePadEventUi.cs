namespace PadOS.Input{
	public delegate void GamePadEventUi<T>(object sender, GamePadEventArgs<T> args);
	public delegate void GamePadEventUi(object sender, GamePadEventArgs args);
}