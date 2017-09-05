using System;
using System.Linq;

namespace PadOS.Views.GamePadOSK {
	public class KeyboardInputSimulator {

		private static void SendKey(string key) {
			if (key.Length == 1)
				foreach (var c in "+^%~(){}".Where(c => key.Contains(c))) {
					key = key.Replace(c.ToString(), "{" + c + "}");
					break;
				}
			System.Windows.Forms.SendKeys.SendWait(key);
		}

		private int _caretIndex;
		private string _text = "";

		public int CaretIndex {
			get => _caretIndex;
			set {
				_caretIndex = value;
				CaretChange?.Invoke(value);
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
			Text = Text.Substring(CaretIndex);
			CaretIndex = 0;
			SendKey("{ENTER}");
		}

		public void ClearText() {
			Text = "";
			CaretIndex = 0;
		}

		public void InsertText(string input, bool send = true) {
			Text = Text.Insert(CaretIndex, input);
			CaretIndex++;
			if (send)
				SendKey(input);
		}

		public void InsertText(char c) {
			InsertText(c.ToString());
		}

		public void RemoveText() {
			if (CaretIndex > -1)
				Text = Text.Remove(CaretIndex, 1);
		}

	}
}
