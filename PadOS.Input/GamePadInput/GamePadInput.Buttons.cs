using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadOS.Input.GamePadInput {
	public partial class GamePadInput {
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
