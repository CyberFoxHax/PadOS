using System;
using System.Linq;

namespace PadOS.Views.GamePadOSK
{
    public class KeyboardInputSimulator
    {
		private void SendKey(string key) {
            if (SimulatorKeyboard == false)
                return;
			if (key.Length == 1)
				foreach (var c in "+^%~(){}".Where(c => key.Contains(c))) {
					key = key.Replace(c.ToString(), "{" + c + "}");
					break;
				}
			System.Windows.Forms.SendKeys.SendWait(key);
		}

        public bool SimulatorKeyboard = true;

		private int _caretIndex;
		private string _text = "";

		public int CaretIndex {
			get => _caretIndex;
			set {
				_caretIndex = value;
				_caretIndex = _caretIndex > Text.Length ? Text.Length : _caretIndex;
				_caretIndex = _caretIndex < 0 ? 0 : _caretIndex;
				CaretChange?.Invoke(_caretIndex);
			}
		}

		public string Text {
			get => _text;
			set {
				_text = value;
				TextChanged?.Invoke(value);
			}
		}

		public event Action<int> CaretChange;
		public event Action<string> TextChanged;

		public void OnDeleteButton() {
			if (CaretIndex > 0 && Text.Length > 0) {
				CaretIndex--;
				RemoveText();
			}
			SendKey("{BS}");
		}

		public void OnMoveRightButton() {
			CaretIndex++;
			SendKey("{RIGHT}");
		}

		public void OnMoveLeftButton() {
			CaretIndex--;
			SendKey("{LEFT}");
		}

		public void OnSpaceButton() {
			InsertText(" ");
		}

		public void OnEnterButton() {
            if (SimulatorKeyboard == false)
                return;
			Text = Text.Substring(CaretIndex);
			CaretIndex = 0;
			SendKey("{ENTER}");
		}

		public void InsertText(string input, bool send = true) {
			Text = Text.Insert(CaretIndex, input);
			CaretIndex++;
			if (send)
				SendKey(input);
		}

        public void SetText(string s) {
            _text = s;
            CaretIndex = s.Length;
        }

        public void InsertText(char c) {
			InsertText(c.ToString());
		}

		public void RemoveText() {
			if (CaretIndex > -1 && CaretIndex < Text.Length) {
				Text = Text.Remove(CaretIndex, 1);
                TextChanged?.Invoke(Text);
            }
		}

	}
}
