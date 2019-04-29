using System;
using System.Threading;
using XInputDotNetPure;

namespace PadOS.Input.GamePadInput {
	public partial class GamePadInput : IDisposable{
		public static GamePadInput StaticInputInstance = new GamePadInput{IsEnabled = true};

		public int PollInterval { get; set; } = 16;
		public double TriggerChangeTolerance { get; set; } = .001;
		public double TriggerDeadZone { get; set; } = .001;
		public double ThumbstickChangeTolerance { get; set; } = .001;
		public double ThumbstickDeadZone {
			get => _thumbstickDeadZone;
			set => _thumbstickDeadZone = value;
		}

		public bool IsEnabled {
			get => _isEnabled;
			set {
				_isEnabled = value;
				if (value == false || _pollThread != null)
					return;
                _suppressEvents = true;
				_pollThread = new Thread(Poll);
				_pollThread.Start();
			}
		}


		private bool _isEnabled;
		private double _thumbstickDeadZone = .001;
		private Thread _pollThread;
        public bool _suppressEvents;
		private readonly GamePadState[] _oldGamePadStates = new GamePadState[4];

		public void Dispose(){
			IsEnabled = false;
		}

		public bool GamePadStateEquals(GamePadState a, GamePadState b){
			return
			//	a.PacketNumber			== b.PacketNumber			 && // verify if packet number is controller specific
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
				Math.Abs(a.ThumbSticks.Left.X	- b.ThumbSticks.Left.X	) < ThumbstickChangeTolerance &&
				Math.Abs(a.ThumbSticks.Left.Y	- b.ThumbSticks.Left.Y	) < ThumbstickChangeTolerance &&
				Math.Abs(a.ThumbSticks.Right.X	- b.ThumbSticks.Right.X	) < ThumbstickChangeTolerance &&
				Math.Abs(a.ThumbSticks.Right.Y	- b.ThumbSticks.Right.Y	) < ThumbstickChangeTolerance &&
				Math.Abs(a.Triggers.Left		- b.Triggers.Left		) < TriggerChangeTolerance &&
				Math.Abs(a.Triggers.Right		- b.Triggers.Right		) < TriggerChangeTolerance
			;
		}

        public void ResetAll() {
            for (int i = 0; i < _oldGamePadStates.Length; i++)
                _oldGamePadStates[0] = default(GamePadState);
        }

		private void Poll(){
			while (IsEnabled){
				for (var i = 0; i < 4; i++){
					var newState = GamePad.GetState((PlayerIndex) i, GamePadDeadZone.None);
					if (GamePadStateEquals(newState, _oldGamePadStates[i]))
						continue;
					GamepadOnStateChanged(_oldGamePadStates[i], newState, i);
					_oldGamePadStates[i] = newState;
				}
                _suppressEvents = false;
                Thread.Sleep(PollInterval);
			}
			_pollThread = null;
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
		}

		private void InvokeUpDown(ButtonState buttonState, int player, GamePadState newState, ref bool isDown, GamePadEvent callbackDown, GamePadEvent callbackUp) {
			if (buttonState == ButtonState.Pressed && isDown == false) {
				isDown = true;
                if (_suppressEvents == false)
                    callbackDown?.Invoke(player, newState);
			}
			else if (buttonState == ButtonState.Released && isDown) {
				isDown = false;
                if (_suppressEvents == false)
                    callbackUp?.Invoke(player, newState);
			}
		}

		private void InvokeThumbChanged(GamePadThumbSticks.StickValue oldValue, GamePadThumbSticks.StickValue newValue, GamePadState newState, int playerIndex, GamePadEvent<Vector2> callback){
			if (Math.Abs(oldValue.X - newValue.X) > ThumbstickChangeTolerance
			||	Math.Abs(oldValue.Y - newValue.Y) > ThumbstickChangeTolerance)
                if(_suppressEvents == false)
				    callback?.Invoke(playerIndex, newState, new Vector2(newValue.X, newValue.Y));
		}

		private void InvokeTriggerChanged(float oldValue, float newValue, GamePadState newState, int playerIndex, GamePadEvent<float> callback) {
			if (Math.Abs(oldValue - newValue) > TriggerChangeTolerance)
                if(_suppressEvents == false)
				    callback?.Invoke(playerIndex, newState, newValue);
		}
	}

}
