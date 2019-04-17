using System.Windows;

namespace PadOS.Input.WpfGamePad {
	public partial class WpfGamePad {

		public static readonly RoutedEvent TriggerLeftChangeEvent = EventManager.RegisterRoutedEvent(
			"TriggerLeftChange", RoutingStrategy.Bubble, typeof(GamePadEventUi<float>), typeof(WpfGamePad));
		public static void AddTriggerLeftChangeHandler(DependencyObject d, GamePadEventUi<float> handler) => (d as UIElement)?.AddHandler(TriggerLeftChangeEvent, handler);
		public static void RemoveTriggerLeftChangeHandler(DependencyObject d, GamePadEventUi<float> handler) => (d as UIElement)?.RemoveHandler(TriggerLeftChangeEvent, handler);

		public static readonly RoutedEvent TriggerRightChangeEvent = EventManager.RegisterRoutedEvent(
			"TriggerRightChange", RoutingStrategy.Bubble, typeof(GamePadEventUi<float>), typeof(WpfGamePad));
		public static void AddTriggerRightChangeHandler(DependencyObject d, GamePadEventUi<float> handler) => (d as UIElement)?.AddHandler(TriggerRightChangeEvent, handler);
		public static void RemoveTriggerRightChangeHandler(DependencyObject d, GamePadEventUi<float> handler) => (d as UIElement)?.RemoveHandler(TriggerRightChangeEvent, handler);



		public static readonly RoutedEvent ThumbLeftChangeEvent = EventManager.RegisterRoutedEvent(
			"ThumbLeftChange", RoutingStrategy.Bubble, typeof(GamePadEventUi<Vector2>), typeof(WpfGamePad));
		public static void AddThumbLeftChangeHandler(DependencyObject d, GamePadEventUi<Vector2> handler) => (d as UIElement)?.AddHandler(ThumbLeftChangeEvent, handler);
		public static void RemoveThumbLeftChangeHandler(DependencyObject d, GamePadEventUi<Vector2> handler) => (d as UIElement)?.RemoveHandler(ThumbLeftChangeEvent, handler);

		public static readonly RoutedEvent ThumbRightChangeEvent = EventManager.RegisterRoutedEvent(
			"ThumbRightChange", RoutingStrategy.Bubble, typeof(GamePadEventUi<Vector2>), typeof(WpfGamePad));
		public static void AddThumbRightChangeHandler(DependencyObject d, GamePadEventUi<Vector2> handler) => (d as UIElement)?.AddHandler(ThumbRightChangeEvent, handler);
		public static void RemoveThumbRightChangeHandler(DependencyObject d, GamePadEventUi<Vector2> handler) => (d as UIElement)?.RemoveHandler(ThumbRightChangeEvent, handler);



		public static readonly RoutedEvent ButtonADownEvent = EventManager.RegisterRoutedEvent(
			"ButtonADown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonADownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonADownEvent, handler);
		public static void RemoveButtonADownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonADownEvent, handler);

		public static readonly RoutedEvent ButtonAUpEvent = EventManager.RegisterRoutedEvent(
			"ButtonAUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonAUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonAUpEvent, handler);
		public static void RemoveButtonAUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonAUpEvent, handler);


		public static readonly RoutedEvent ButtonBDownEvent = EventManager.RegisterRoutedEvent(
			"ButtonBDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonBDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonBDownEvent, handler);
		public static void RemoveButtonBDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonBDownEvent, handler);

		public static readonly RoutedEvent ButtonBUpEvent = EventManager.RegisterRoutedEvent(
			"ButtonBUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonBUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonBUpEvent, handler);
		public static void RemoveButtonBUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonBUpEvent, handler);


		public static readonly RoutedEvent ButtonXDownEvent = EventManager.RegisterRoutedEvent(
			"ButtonXDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonXDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonXDownEvent, handler);
		public static void RemoveButtonXDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonXDownEvent, handler);

		public static readonly RoutedEvent ButtonXUpEvent = EventManager.RegisterRoutedEvent(
			"ButtonXUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonXUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonXUpEvent, handler);
		public static void RemoveButtonXUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonXUpEvent, handler);


		public static readonly RoutedEvent ButtonYDownEvent = EventManager.RegisterRoutedEvent(
			"ButtonYDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonYDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonYDownEvent, handler);
		public static void RemoveButtonYDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonYDownEvent, handler);

		public static readonly RoutedEvent ButtonYUpEvent = EventManager.RegisterRoutedEvent(
			"ButtonYUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonYUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonYUpEvent, handler);
		public static void RemoveButtonYUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonYUpEvent, handler);


		public static readonly RoutedEvent ButtonBackDownEvent = EventManager.RegisterRoutedEvent(
			"ButtonBackDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonBackDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonBackDownEvent, handler);
		public static void RemoveButtonBackDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonBackDownEvent, handler);

		public static readonly RoutedEvent ButtonBackUpEvent = EventManager.RegisterRoutedEvent(
			"ButtonBackUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonBackUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonBackUpEvent, handler);
		public static void RemoveButtonBackUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonBackUpEvent, handler);


		public static readonly RoutedEvent ButtonGuideDownEvent = EventManager.RegisterRoutedEvent(
			"ButtonGuideDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonGuideDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonGuideDownEvent, handler);
		public static void RemoveButtonGuideDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonGuideDownEvent, handler);

		public static readonly RoutedEvent ButtonGuideUpEvent = EventManager.RegisterRoutedEvent(
			"ButtonGuideUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonGuideUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonGuideUpEvent, handler);
		public static void RemoveButtonGuideUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonGuideUpEvent, handler);


		public static readonly RoutedEvent ButtonLeftShoulderDownEvent = EventManager.RegisterRoutedEvent(
			"ButtonLeftShoulderDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonLeftShoulderDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonLeftShoulderDownEvent, handler);
		public static void RemoveButtonLeftShoulderDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonLeftShoulderDownEvent, handler);

		public static readonly RoutedEvent ButtonLeftShoulderUpEvent = EventManager.RegisterRoutedEvent(
			"ButtonLeftShoulderUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonLeftShoulderUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonLeftShoulderUpEvent, handler);
		public static void RemoveButtonLeftShoulderUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonLeftShoulderUpEvent, handler);


		public static readonly RoutedEvent ButtonLeftStickDownEvent = EventManager.RegisterRoutedEvent(
			"ButtonLeftStickDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonLeftStickDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonLeftStickDownEvent, handler);
		public static void RemoveButtonLeftStickDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonLeftStickDownEvent, handler);

		public static readonly RoutedEvent ButtonLeftStickUpEvent = EventManager.RegisterRoutedEvent(
			"ButtonLeftStickUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonLeftStickUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonLeftStickUpEvent, handler);
		public static void RemoveButtonLeftStickUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonLeftStickUpEvent, handler);


		public static readonly RoutedEvent ButtonRightShoulderDownEvent = EventManager.RegisterRoutedEvent(
			"ButtonRightShoulderDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonRightShoulderDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonRightShoulderDownEvent, handler);
		public static void RemoveButtonRightShoulderDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonRightShoulderDownEvent, handler);

		public static readonly RoutedEvent ButtonRightShoulderUpEvent = EventManager.RegisterRoutedEvent(
			"ButtonRightShoulderUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonRightShoulderUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonRightShoulderUpEvent, handler);
		public static void RemoveButtonRightShoulderUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonRightShoulderUpEvent, handler);


		public static readonly RoutedEvent ButtonRightStickDownEvent = EventManager.RegisterRoutedEvent(
			"ButtonRightStickDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonRightStickDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonRightStickDownEvent, handler);
		public static void RemoveButtonRightStickDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonRightStickDownEvent, handler);

		public static readonly RoutedEvent ButtonRightStickUpEvent = EventManager.RegisterRoutedEvent(
			"ButtonRightStickUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonRightStickUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonRightStickUpEvent, handler);
		public static void RemoveButtonRightStickUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonRightStickUpEvent, handler);


		public static readonly RoutedEvent ButtonStartDownEvent = EventManager.RegisterRoutedEvent(
			"ButtonStartDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonStartDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonStartDownEvent, handler);
		public static void RemoveButtonStartDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonStartDownEvent, handler);

		public static readonly RoutedEvent ButtonStartUpEvent = EventManager.RegisterRoutedEvent(
			"ButtonStartUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddButtonStartUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(ButtonStartUpEvent, handler);
		public static void RemoveButtonStartUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(ButtonStartUpEvent, handler);


		public static readonly RoutedEvent DPadLeftDownEvent = EventManager.RegisterRoutedEvent(
			"DPadLeftDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddDPadLeftDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(DPadLeftDownEvent, handler);
		public static void RemoveDPadLeftDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(DPadLeftDownEvent, handler);

		public static readonly RoutedEvent DPadLeftUpEvent = EventManager.RegisterRoutedEvent(
			"DPadLeftUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddDPadLeftUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(DPadLeftUpEvent, handler);
		public static void RemoveDPadLeftUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(DPadLeftUpEvent, handler);


		public static readonly RoutedEvent DPadRightDownEvent = EventManager.RegisterRoutedEvent(
			"DPadRightDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddDPadRightDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(DPadRightDownEvent, handler);
		public static void RemoveDPadRightDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(DPadRightDownEvent, handler);

		public static readonly RoutedEvent DPadRightUpEvent = EventManager.RegisterRoutedEvent(
			"DPadRightUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddDPadRightUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(DPadRightUpEvent, handler);
		public static void RemoveDPadRightUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(DPadRightUpEvent, handler);


		public static readonly RoutedEvent DPadUpDownEvent = EventManager.RegisterRoutedEvent(
			"DPadUpDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddDPadUpDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(DPadUpDownEvent, handler);
		public static void RemoveDPadUpDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(DPadUpDownEvent, handler);

		public static readonly RoutedEvent DPadUpUpEvent = EventManager.RegisterRoutedEvent(
			"DPadUpUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddDPadUpUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(DPadUpUpEvent, handler);
		public static void RemoveDPadUpUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(DPadUpUpEvent, handler);


		public static readonly RoutedEvent DPadDownDownEvent = EventManager.RegisterRoutedEvent(
			"DPadDownDown", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddDPadDownDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(DPadDownDownEvent, handler);
		public static void RemoveDPadDownDownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(DPadDownDownEvent, handler);

		public static readonly RoutedEvent DPadDownUpEvent = EventManager.RegisterRoutedEvent(
			"DPadDownUp", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));
		public static void AddDPadDownUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler(DPadDownUpEvent, handler);
		public static void RemoveDPadDownUpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler(DPadDownUpEvent, handler);

		private static readonly RoutedEvent[] ButtonEvents = {
			ButtonADownEvent, ButtonAUpEvent,
			ButtonBDownEvent, ButtonBUpEvent,
			ButtonXDownEvent, ButtonXUpEvent,
			ButtonYDownEvent, ButtonYUpEvent,
			ButtonBackDownEvent, ButtonBackUpEvent,
			ButtonGuideDownEvent, ButtonGuideUpEvent,
			ButtonLeftShoulderDownEvent, ButtonLeftShoulderUpEvent,
			ButtonLeftStickDownEvent, ButtonLeftStickUpEvent,
			ButtonRightShoulderDownEvent, ButtonRightShoulderUpEvent,
			ButtonRightStickDownEvent, ButtonRightStickUpEvent,
			ButtonStartDownEvent, ButtonStartUpEvent,
			DPadLeftDownEvent, DPadLeftUpEvent,
			DPadRightDownEvent, DPadRightUpEvent,
			DPadUpDownEvent, DPadUpUpEvent,
			DPadDownDownEvent, DPadDownUpEvent
		};

		private static readonly RoutedEvent[] ThumbstickEvents = {
			ThumbLeftChangeEvent,
			ThumbRightChangeEvent
		};

		private static readonly RoutedEvent[] TriggerEvents = {
			TriggerLeftChangeEvent,
			TriggerRightChangeEvent
		};
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
	$"public static readonly RoutedEvent {p}DownEvent = EventManager.RegisterRoutedEvent(\r\n" +
	$"	\"{p}Down\", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));\r\n" +
	$"public static void Add{p}DownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler({p}DownEvent, handler);\r\n" +
	$"public static void Remove{p}DownHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler({p}DownEvent, handler);\r\n\r\n" +

	$"public static readonly RoutedEvent {p}UpEvent = EventManager.RegisterRoutedEvent(\r\n" +
	$"	\"{p}Up\", RoutingStrategy.Bubble, typeof(GamePadEventUi), typeof(WpfGamePad));\r\n" +
	$"public static void Add{p}UpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.AddHandler({p}UpEvent, handler);\r\n" +
	$"public static void Remove{p}UpHandler(DependencyObject d, GamePadEventUi handler) => (d as UIElement)?.RemoveHandler({p}UpEvent, handler);\r\n"
).ToArray());
 */
