using System;

namespace PadOS.Views {
	public partial class AdornerTest {
		public AdornerTest() {
			InitializeComponent();

			var wrapper = Input.WPFGamepad.Register(this);

			wrapper.ThumbLeftChange			+= (s, a)=>Console.WriteLine(@"ThumbLeft Change"    + a.Value);
			wrapper.ThumbRightChange		+= (s, a)=>Console.WriteLine(@"ThumbRight Change"   + a.Value);
			wrapper.TriggerLeftChange		+= (s, a)=>Console.WriteLine(@"TriggerLeft Change"  + a.Value);
			wrapper.TriggerRightChange		+= (s, a)=>Console.WriteLine(@"TriggerRight Change" + a.Value);

			Input.WPFDirectionalControls.Register(this);
		}
	}
}
