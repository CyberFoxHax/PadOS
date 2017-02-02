using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadOS {
	public static class GlobalEvents {
		public static void Init(){
			Input.WPFGamepad.XInput.ButtonGuideDown += XInputOnButtonGuideDown;
		}

		public delegate void InterfaceChangeEvent(bool isOpen);

		public static bool InterfaceIsOpen;
		public static event InterfaceChangeEvent HomeOpen;
		public static event InterfaceChangeEvent HomeClose;

		private static void XInputOnButtonGuideDown(XInputDotNetPure.PlayerIndex player, XInputDotNetPure.GamePadState state){
			if (InterfaceIsOpen){
				if (HomeClose != null)
					HomeClose(InterfaceIsOpen);
			}
			else{
				if (HomeOpen != null)
					HomeOpen(InterfaceIsOpen);
			}
			InterfaceIsOpen = !InterfaceIsOpen;
		}
	}
}
