using System;
using XInputDotNetPure;

namespace PadOS.Input {
	public class GamePadInput {
		public GamePadInput(){
			Enable();

			_isRunning = true;
			new System.Threading.Thread(Poll).Start();
		}

		~GamePadInput(){
			_isRunning = false;
		}

		private bool _isActive;

		public void Enable(){
			_isActive = true;
		}

		public void Disable(){
			_isActive = false;
		}

		public void Dispose() {
			_isActive = false;
			_isRunning = false;
		}

		private static readonly PlayerIndex[] PlayerIndices = { PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four };

		private readonly GamePadState[] _oldGamePadStates = new GamePadState[4];

		public GamePadDeadZone DeadZone { get; set; }

		public static bool GamePadStateEquals(GamePadState a, GamePadState b){
			const float tolerance = 0.001f;
			return
				a.PacketNumber			== b.PacketNumber			 && // verify if packet number is controller specific
				a.Buttons.LeftShoulder	== b.Buttons.LeftShoulder	 && 
				a.Buttons.A				== b.Buttons.A				 &&
				a.Buttons.B				== b.Buttons.B				 &&
				a.Buttons.Back			== b.Buttons.Back			 &&
				a.Buttons.Guide			== b.Buttons.Guide			 &&
				a.Buttons.LeftShoulder	== b.Buttons.LeftShoulder	 &&
				a.Buttons.LeftStick		== b.Buttons.LeftStick		 &&
				a.Buttons.RightShoulder	== b.Buttons.RightShoulder	 &&
				a.Buttons.RightStick	== b.Buttons.RightStick		 &&
				a.Buttons.Start			== b.Buttons.Start			 &&
				a.Buttons.X				== b.Buttons.X				 &&
				a.Buttons.Y				== b.Buttons.Y				 &&
				a.DPad.Left				== b.DPad.Left				 &&
				a.DPad.Right			== b.DPad.Right				 &&
				a.DPad.Up				== b.DPad.Up				 &&
				a.DPad.Down				== b.DPad.Down				 &&
				Math.Abs(a.ThumbSticks.Left.X	- b.ThumbSticks.Left.X	) < tolerance &&
				Math.Abs(a.ThumbSticks.Left.Y	- b.ThumbSticks.Left.Y	) < tolerance &&
				Math.Abs(a.ThumbSticks.Right.X	- b.ThumbSticks.Right.X	) < tolerance &&
				Math.Abs(a.ThumbSticks.Right.Y	- b.ThumbSticks.Right.Y	) < tolerance &&
				Math.Abs(a.Triggers.Left		- b.Triggers.Left		) < tolerance &&
				Math.Abs(a.Triggers.Right		- b.Triggers.Right		) < tolerance
			;
		}

		private void Poll(){
			while (_isRunning){
				for (var i = 0; i < 4; i++){
					var newState = GamePad.GetState(PlayerIndices[i], GamePadDeadZone.None);
					
					if (GamePadStateEquals(newState, _oldGamePadStates[i])) continue;

					GamepadOnStateChanged(_oldGamePadStates[i], newState, i);

					_oldGamePadStates[i] = newState;
				}

				System.Threading.Thread.Sleep(16);
			}
		}

		private void GamepadOnStateChanged(GamePadState oldState, GamePadState newState, int playerIndex){
			InvokeUpDown(newState.Buttons.A				, playerIndex, newState, ref _isButtonADown				, ButtonADown				, ButtonAUp				);
			InvokeUpDown(newState.Buttons.B				, playerIndex, newState, ref _isButtonBDown				, ButtonBDown				, ButtonBUp				);
			InvokeUpDown(newState.Buttons.X				, playerIndex, newState, ref _isButtonXDown				, ButtonXDown				, ButtonXUp				);
			InvokeUpDown(newState.Buttons.Y				, playerIndex, newState, ref _isButtonYDown				, ButtonYDown				, ButtonYUp				);
			InvokeUpDown(newState.Buttons.Back			, playerIndex, newState, ref _isButtonBackDown			, ButtonBackDown			, ButtonBackUp			);
			InvokeUpDown(newState.Buttons.Guide			, playerIndex, newState, ref _isButtonGuideDown			, ButtonGuideDown			, ButtonGuideUp			);
			InvokeUpDown(newState.Buttons.LeftShoulder	, playerIndex, newState, ref _isButtonLeftShoulderDown	, ButtonLeftShoulderDown	, ButtonLeftShoulderUp	);
			InvokeUpDown(newState.Buttons.LeftStick		, playerIndex, newState, ref _isButtonLeftStickDown		, ButtonLeftStickDown		, ButtonLeftStickUp		);
			InvokeUpDown(newState.Buttons.RightShoulder	, playerIndex, newState, ref _isButtonRightShoulderDown	, ButtonRightShoulderDown	, ButtonRightShoulderUp	);
			InvokeUpDown(newState.Buttons.RightStick	, playerIndex, newState, ref _isButtonRightStickDown	, ButtonRightStickDown		, ButtonRightStickUp	);
			InvokeUpDown(newState.Buttons.Start			, playerIndex, newState, ref _isButtonStartDown			, ButtonStartDown			, ButtonStartUp			);
			InvokeUpDown(newState.DPad.Left				, playerIndex, newState, ref _isDPadLeftDown			, DPadLeftDown				, DPadLeftUp			);
			InvokeUpDown(newState.DPad.Right			, playerIndex, newState, ref _isDPadRightDown			, DPadRightDown				, DPadRightUp			);
			InvokeUpDown(newState.DPad.Up				, playerIndex, newState, ref _isDPadUpDown				, DPadUpDown				, DPadUpUp				);
			InvokeUpDown(newState.DPad.Down				, playerIndex, newState, ref _isDPadDownDown			, DPadDownDown				, DPadDownUp			);

			InvokeThumbChanged(oldState.ThumbSticks.Left , newState.ThumbSticks.Left , newState, playerIndex, ThumbLeftChange );
			InvokeThumbChanged(oldState.ThumbSticks.Right, newState.ThumbSticks.Right, newState, playerIndex, ThumbRightChange);

			InvokeTriggerChanged(oldState.Triggers.Left , newState.Triggers.Left , newState, playerIndex, TriggerLeftChange );
			InvokeTriggerChanged(oldState.Triggers.Right, newState.Triggers.Right, newState, playerIndex, TriggerRightChange);

			if (Changed != null)
				Changed(PlayerIndices[playerIndex], newState);
		}

		public delegate void GamePadEvent(PlayerIndex player, GamePadState state);
		public delegate void GamePadEvent<in T>(PlayerIndex player, GamePadState state, T value);

		private static void InvokeUpDown(ButtonState buttonState, int player, GamePadState newState, ref bool isDown, GamePadEvent callbackDown, GamePadEvent callbackUp) {
			if (buttonState == ButtonState.Pressed && isDown == false) {
				isDown = true;
				if (callbackDown != null) callbackDown(PlayerIndices[player], newState);
			}
			else if (buttonState == ButtonState.Released && isDown) {
				isDown = false;
				if (callbackUp != null) callbackUp(PlayerIndices[player], newState);
			}
		}

		private static void InvokeThumbChanged(GamePadThumbSticks.StickValue oldValue, GamePadThumbSticks.StickValue newValue, GamePadState newState, int playerIndex, GamePadEvent<Vector2> callback){
			if (Math.Abs(oldValue.X - newValue.X) > 0.001
			||	Math.Abs(oldValue.Y - newValue.Y) > 0.001) {
				
				if (callback != null)
					callback(PlayerIndices[playerIndex], newState, new Vector2(newValue.X, newValue.Y));
			}
		}

		private static void InvokeTriggerChanged(float oldValue, float newValue, GamePadState newState, int playerIndex, GamePadEvent<float> callback) {
			if (Math.Abs(oldValue - newValue) > 0.001) {
				if (callback != null)
					callback(PlayerIndices[playerIndex], newState, newValue);
			}
		}

		private bool _isRunning;
		private bool _isButtonADown;
		private bool _isButtonBDown;
		private bool _isButtonXDown;
		private bool _isButtonYDown;
		private bool _isButtonBackDown;
		private bool _isButtonGuideDown;
		private bool _isButtonLeftShoulderDown;
		private bool _isButtonLeftStickDown;
		private bool _isButtonRightShoulderDown;
		private bool _isButtonRightStickDown;
		private bool _isButtonStartDown;
		private bool _isDPadLeftDown;
		private bool _isDPadRightDown;
		private bool _isDPadUpDown;
		private bool _isDPadDownDown;

		public event GamePadEvent Changed;
		public event GamePadEvent ButtonADown;
		public event GamePadEvent ButtonAUp;
		public event GamePadEvent ButtonBDown;
		public event GamePadEvent ButtonBUp;
		public event GamePadEvent ButtonXDown;
		public event GamePadEvent ButtonXUp;
		public event GamePadEvent ButtonYDown;
		public event GamePadEvent ButtonYUp;
		public event GamePadEvent ButtonBackDown;
		public event GamePadEvent ButtonBackUp;
		public event GamePadEvent ButtonGuideDown;
		public event GamePadEvent ButtonGuideUp;
		public event GamePadEvent ButtonLeftShoulderDown;
		public event GamePadEvent ButtonLeftShoulderUp;
		public event GamePadEvent ButtonLeftStickDown;
		public event GamePadEvent ButtonLeftStickUp;
		public event GamePadEvent ButtonRightShoulderDown;
		public event GamePadEvent ButtonRightShoulderUp;
		public event GamePadEvent ButtonRightStickDown;
		public event GamePadEvent ButtonRightStickUp;
		public event GamePadEvent ButtonStartDown;
		public event GamePadEvent ButtonStartUp;
		public event GamePadEvent DPadLeftDown;
		public event GamePadEvent DPadLeftUp;
		public event GamePadEvent DPadRightDown;
		public event GamePadEvent DPadRightUp;
		public event GamePadEvent DPadUpDown;
		public event GamePadEvent DPadUpUp;
		public event GamePadEvent DPadDownDown;
		public event GamePadEvent DPadDownUp;
		public event GamePadEvent<Vector2> ThumbLeftChange;
		public event GamePadEvent<Vector2> ThumbRightChange;
		public event GamePadEvent<float> TriggerLeftChange;
		public event GamePadEvent<float> TriggerRightChange;
	}

}
