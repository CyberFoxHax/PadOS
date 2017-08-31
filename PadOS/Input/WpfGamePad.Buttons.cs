using System.Windows;
using XInputDotNetPure;

namespace PadOS.Input {
	public partial class WpfGamePad {
		public static readonly RoutedEvent TriggerLeftChange = EventManager.RegisterRoutedEvent(
			"TriggerLeftChange", RoutingStrategy.Bubble, typeof(GamePadEventUi<float>), typeof(WpfGamePad));
		public static void AddTriggerLeftChangeHandler(DependencyObject d, GamePadEventUi<float> handler) => (d as UIElement)?.AddHandler(TriggerLeftChange, handler);
		public static void RemoveTriggerLeftChangeHandler(DependencyObject d, GamePadEventUi<float> handler) => (d as UIElement)?.RemoveHandler(TriggerLeftChange, handler);

		public static readonly RoutedEvent TriggerRightChange = EventManager.RegisterRoutedEvent(
			"TriggerRightChange", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddTriggerRightChangeHandler(DependencyObject d, GamePadEventUi<Vector2> handler) => (d as UIElement)?.AddHandler(TriggerRightChange, handler);
		public static void RemoveTriggerRightChangeHandler(DependencyObject d, GamePadEventUi<Vector2> handler) => (d as UIElement)?.RemoveHandler(TriggerRightChange, handler);



		public static readonly RoutedEvent ThumbLeftChange = EventManager.RegisterRoutedEvent(
			"ThumbLeftChange", RoutingStrategy.Bubble, typeof(GamePadEventUi<Vector2>), typeof(WpfGamePad));
		public static void AddThumbLeftChangeHandler(DependencyObject d, GamePadEventUi<Vector2> handler) => (d as UIElement)?.AddHandler(ThumbLeftChange, handler);
		public static void RemoveThumbLeftChangeHandler(DependencyObject d, GamePadEventUi<Vector2> handler) => (d as UIElement)?.RemoveHandler(ThumbLeftChange, handler);

		public static readonly RoutedEvent ThumbRightChange = EventManager.RegisterRoutedEvent(
			"ThumbRightChange", RoutingStrategy.Bubble, typeof(GamePadEventUi<Vector2>), typeof(WpfGamePad));
		public static void AddThumbRightChangeHandler(DependencyObject d, GamePadEventUi<Vector2> handler) => (d as UIElement)?.AddHandler(ThumbRightChange, handler);
		public static void RemoveThumbRightChangeHandler(DependencyObject d, GamePadEventUi<Vector2> handler) => (d as UIElement)?.RemoveHandler(ThumbRightChange, handler);



		public static readonly RoutedEvent ButtonADown = EventManager.RegisterRoutedEvent(
			"ButtonADown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonADownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonADown, handler);
		public static void RemoveButtonADownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonADown, handler);

		public static readonly RoutedEvent ButtonAUp = EventManager.RegisterRoutedEvent(
			"ButtonAUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonAUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonAUp, handler);
		public static void RemoveButtonAUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonAUp, handler);


		public static readonly RoutedEvent ButtonBDown = EventManager.RegisterRoutedEvent(
			"ButtonBDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonBDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonBDown, handler);
		public static void RemoveButtonBDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonBDown, handler);

		public static readonly RoutedEvent ButtonBUp = EventManager.RegisterRoutedEvent(
			"ButtonBUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonBUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonBUp, handler);
		public static void RemoveButtonBUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonBUp, handler);


		public static readonly RoutedEvent ButtonXDown = EventManager.RegisterRoutedEvent(
			"ButtonXDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonXDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonXDown, handler);
		public static void RemoveButtonXDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonXDown, handler);

		public static readonly RoutedEvent ButtonXUp = EventManager.RegisterRoutedEvent(
			"ButtonXUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonXUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonXUp, handler);
		public static void RemoveButtonXUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonXUp, handler);


		public static readonly RoutedEvent ButtonYDown = EventManager.RegisterRoutedEvent(
			"ButtonYDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonYDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonYDown, handler);
		public static void RemoveButtonYDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonYDown, handler);

		public static readonly RoutedEvent ButtonYUp = EventManager.RegisterRoutedEvent(
			"ButtonYUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonYUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonYUp, handler);
		public static void RemoveButtonYUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonYUp, handler);


		public static readonly RoutedEvent ButtonBackDown = EventManager.RegisterRoutedEvent(
			"ButtonBackDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonBackDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonBackDown, handler);
		public static void RemoveButtonBackDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonBackDown, handler);

		public static readonly RoutedEvent ButtonBackUp = EventManager.RegisterRoutedEvent(
			"ButtonBackUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonBackUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonBackUp, handler);
		public static void RemoveButtonBackUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonBackUp, handler);


		public static readonly RoutedEvent ButtonGuideDown = EventManager.RegisterRoutedEvent(
			"ButtonGuideDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonGuideDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonGuideDown, handler);
		public static void RemoveButtonGuideDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonGuideDown, handler);

		public static readonly RoutedEvent ButtonGuideUp = EventManager.RegisterRoutedEvent(
			"ButtonGuideUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonGuideUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonGuideUp, handler);
		public static void RemoveButtonGuideUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonGuideUp, handler);


		public static readonly RoutedEvent ButtonLeftShoulderDown = EventManager.RegisterRoutedEvent(
			"ButtonLeftShoulderDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonLeftShoulderDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonLeftShoulderDown, handler);
		public static void RemoveButtonLeftShoulderDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonLeftShoulderDown, handler);

		public static readonly RoutedEvent ButtonLeftShoulderUp = EventManager.RegisterRoutedEvent(
			"ButtonLeftShoulderUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonLeftShoulderUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonLeftShoulderUp, handler);
		public static void RemoveButtonLeftShoulderUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonLeftShoulderUp, handler);


		public static readonly RoutedEvent ButtonLeftStickDown = EventManager.RegisterRoutedEvent(
			"ButtonLeftStickDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonLeftStickDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonLeftStickDown, handler);
		public static void RemoveButtonLeftStickDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonLeftStickDown, handler);

		public static readonly RoutedEvent ButtonLeftStickUp = EventManager.RegisterRoutedEvent(
			"ButtonLeftStickUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonLeftStickUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonLeftStickUp, handler);
		public static void RemoveButtonLeftStickUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonLeftStickUp, handler);


		public static readonly RoutedEvent ButtonRightShoulderDown = EventManager.RegisterRoutedEvent(
			"ButtonRightShoulderDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonRightShoulderDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonRightShoulderDown, handler);
		public static void RemoveButtonRightShoulderDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonRightShoulderDown, handler);

		public static readonly RoutedEvent ButtonRightShoulderUp = EventManager.RegisterRoutedEvent(
			"ButtonRightShoulderUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonRightShoulderUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonRightShoulderUp, handler);
		public static void RemoveButtonRightShoulderUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonRightShoulderUp, handler);


		public static readonly RoutedEvent ButtonRightStickDown = EventManager.RegisterRoutedEvent(
			"ButtonRightStickDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonRightStickDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonRightStickDown, handler);
		public static void RemoveButtonRightStickDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonRightStickDown, handler);

		public static readonly RoutedEvent ButtonRightStickUp = EventManager.RegisterRoutedEvent(
			"ButtonRightStickUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonRightStickUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonRightStickUp, handler);
		public static void RemoveButtonRightStickUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonRightStickUp, handler);


		public static readonly RoutedEvent ButtonStartDown = EventManager.RegisterRoutedEvent(
			"ButtonStartDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonStartDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonStartDown, handler);
		public static void RemoveButtonStartDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonStartDown, handler);

		public static readonly RoutedEvent ButtonStartUp = EventManager.RegisterRoutedEvent(
			"ButtonStartUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonStartUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonStartUp, handler);
		public static void RemoveButtonStartUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonStartUp, handler);


		public static readonly RoutedEvent DPadLeftDown = EventManager.RegisterRoutedEvent(
			"DPadLeftDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddDPadLeftDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(DPadLeftDown, handler);
		public static void RemoveDPadLeftDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(DPadLeftDown, handler);

		public static readonly RoutedEvent DPadLeftUp = EventManager.RegisterRoutedEvent(
			"DPadLeftUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddDPadLeftUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(DPadLeftUp, handler);
		public static void RemoveDPadLeftUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(DPadLeftUp, handler);


		public static readonly RoutedEvent DPadRightDown = EventManager.RegisterRoutedEvent(
			"DPadRightDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddDPadRightDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(DPadRightDown, handler);
		public static void RemoveDPadRightDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(DPadRightDown, handler);

		public static readonly RoutedEvent DPadRightUp = EventManager.RegisterRoutedEvent(
			"DPadRightUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddDPadRightUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(DPadRightUp, handler);
		public static void RemoveDPadRightUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(DPadRightUp, handler);


		public static readonly RoutedEvent DPadUpDown = EventManager.RegisterRoutedEvent(
			"DPadUpDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddDPadUpDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(DPadUpDown, handler);
		public static void RemoveDPadUpDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(DPadUpDown, handler);

		public static readonly RoutedEvent DPadUpUp = EventManager.RegisterRoutedEvent(
			"DPadUpUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddDPadUpUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(DPadUpUp, handler);
		public static void RemoveDPadUpUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(DPadUpUp, handler);


		public static readonly RoutedEvent DPadDownDown = EventManager.RegisterRoutedEvent(
			"DPadDownDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddDPadDownDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(DPadDownDown, handler);
		public static void RemoveDPadDownDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(DPadDownDown, handler);

		public static readonly RoutedEvent DPadDownUp = EventManager.RegisterRoutedEvent(
			"DPadDownUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddDPadDownUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(DPadDownUp, handler);
		public static void RemoveDPadDownUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(DPadDownUp, handler);
	}
}
/*
 var trout = string.Join("\r\n\r\n", new[]{
	"ButtonA",
	"ButtonB",
	"ButtonX",
	"ButtonY",
	"ButtonBack",
	"ButtonGuide",
	"ButtonLeftShoulder",
	"ButtonLeftStick",
	"ButtonRightShoulder",
	"ButtonRightStick",
	"ButtonStart",
	"DPadLeft",
	"DPadRight",
	"DPadUp",
	"DPadDown"
}.Select(p =>
	$"public static readonly RoutedEvent {p}Down = EventManager.RegisterRoutedEvent(\r\n" +
	$"	\"{p}Down\", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));\r\n" +
	$"public static void Add{p}DownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler({p}Down, handler);\r\n" +
	$"public static void Remove{p}DownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler({p}Down, handler);\r\n\r\n" +

	$"public static readonly RoutedEvent {p}Up = EventManager.RegisterRoutedEvent(\r\n" +
	$"	\"{p}Up\", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));\r\n" +
	$"public static void Add{p}UpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler({p}Up, handler);\r\n" +
	$"public static void Remove{p}UpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler({p}Up, handler);\r\n"
).ToArray());
 */