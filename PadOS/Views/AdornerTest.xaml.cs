using System;

namespace PadOS.Views {
	public partial class AdornerTest {
		public AdornerTest() {
			InitializeComponent();
			ButtonBegin.Focus();

			Input.WPFDirectionalControls.Register(this);
		}
	}
}
