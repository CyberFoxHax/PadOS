using System.Linq;
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

		readonly GamePadState[] _gamePadStates = new GamePadState[4];
		readonly uint[] _lastPacketNumbers = new uint[4];

		public GamePadDeadZone DeadZone { get; set; }

		public static bool GamePadStateEquals(GamePadState a, GamePadState b){
			const float tolerance = 0.001f;
			return
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
				System.Math.Abs(a.ThumbSticks.Left.X	- b.ThumbSticks.Left.X	) < tolerance &&
				System.Math.Abs(a.ThumbSticks.Left.Y	- b.ThumbSticks.Left.Y	) < tolerance &&
				System.Math.Abs(a.ThumbSticks.Right.X	- b.ThumbSticks.Right.X	) < tolerance &&
				System.Math.Abs(a.ThumbSticks.Right.Y	- b.ThumbSticks.Right.Y	) < tolerance &&
				System.Math.Abs(a.Triggers.Left			- b.Triggers.Left		) < tolerance &&
				System.Math.Abs(a.Triggers.Right		- b.Triggers.Right		) < tolerance
			;
		}

		private void Poll(){
			var oldGamePadStates = new GamePadState[4];
			while (_isRunning){
				for (var i = 0; i < 4; i++){
					_gamePadStates[i] = GamePad.GetState(PlayerIndices[i], GamePadDeadZone.None);

					if (_gamePadStates[i].PacketNumber == _lastPacketNumbers[i])
						continue;
					_lastPacketNumbers[i] = _gamePadStates[i].PacketNumber;

					if (GamePadStateEquals(oldGamePadStates[i], _gamePadStates[i]) == false){
						GamepadOnStateChanged();
						GamePadChange(_gamePadStates[i]);
					}
				}

				System.Threading.Thread.Sleep(16);
			}
		}

		private GamePadState _lastState;

		private void GamepadOnStateChanged(){
		}

		private bool _isRunning;

		static GamePadInput(){
			
		}

		public event System.Action<GamePadState> GamePadChange;
	}
}
