using System;
using System.Collections.Generic;
using System.Linq;

namespace PadOS.Views.GamePadOSK.Controls {
	public partial class Dial  {
		public Dial() {
			InitializeComponent();
			Background = null;

			var children = Children.ToArray();

			_elmGrid = new DialItem[3,3];
			for (int y = 0, i = 0;	y < 3; y++)
			for (var x = 0;			x < 3; x++)
				_elmGrid[x, y] = children[i++];

			var seq = BaseChars;
			for (int i = 0, c = 0; i < seq.Length; i += 4, c++)
				children[c].SetChars(seq.Substring(i, 4));

			FocusBlock(children[4]);
		}

		private IEnumerable<DialItem> Children => Wrapper.Children.OfType<DialItem>();
		private readonly DialItem[,] _elmGrid;
		private DialItem _activeElement;

		private void FocusBlock(DialItem elm){
			if (_activeElement != null)
				_activeElement.IsActive = false;

			_activeElement = elm;
			elm.IsActive = true;
		}

		public void SwitchUppercase(){
			var children = Children.ToArray();
			var seq = BaseCharsUpper;
			for (int i = 0, c = 0; i < seq.Length; i += 4, c++)
				children[c].SetChars(seq.Substring(i, 4));
		}

		public void SwitchLowercase(){
			var children = Children.ToArray();
			var seq = BaseChars;
			for (int i = 0, c = 0; i < seq.Length; i += 4, c++)
				children[c].SetChars(seq.Substring(i, 4));
		}

		public void SwitchSymbols(){
			var children = Children.ToArray();
			var seq = SymbolChars;
			for (int i = 0, c = 0; i < seq.Length; i += 4, c++)
				children[c].SetChars(seq.Substring(i, 4));
		}

		public void SetBlockFocus(Vector2 value){
			if (value.GetLength() < 0.5)
				value = new Vector2();
			value = value.GetNormalized();
			FocusBlock(_elmGrid[
				(int) (value.X + 1),
				(int) (-value.Y + 1)
			]);
		}

		public char GetChar(Vector2 value){
			return _activeElement.GetChar(
				(int) -value.X,
				(int) value.Y
			);
		}

		public static readonly string BaseChars;
		public static readonly string BaseCharsUpper;
		public static readonly string SymbolChars;

		static Dial(){
			BaseChars = QwertySequence;
			BaseCharsUpper = BaseChars.ToUpper();
			SymbolChars = "%€|&" + "+-*/" + "=[]\\" + "^<>~" + "'!?." + "°{}¥" + "\":;@" + "_#,€" + "$()£";
		}

		private const string QwertySequence = "1qew" + "2ryt" + "3uoi" + "4ads" + "5fhg" + "6jlk" + "7zcx" + "8vnb" + "9m0p";
	}
}
