using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PadOS.Navigation;
using PadOS.Views.GamePadOSK;
using PadOS.Views.Settings;

namespace PadOS.ViewModels.FunctionButtons {
	public static class PadOsInternalFunctions {

		public static Action GetFunction(string key){
			return Functions.ContainsKey(key) ? Functions[key] : null;
		}

		[Key("OpenOsk")]
		public static void OpenOsk() {
			Navigator.OpenWindow<Osk>();
		}

		[Key("OpenSettings")]
		public static void OpenSettings() {
			Navigator.OpenWindow<Settings>();
		}

		/////////////////////////////////////

		private static readonly Dictionary<string, Action> Functions;

		static PadOsInternalFunctions(){
			var type = typeof (PadOsInternalFunctions);
			var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);
			Functions = methods.Select(p =>{
				var attr = p.GetCustomAttribute(typeof (KeyAttribute)) as KeyAttribute;
				if (attr == null) return null;
				return new{
					Key = attr.FunctionKey,
					Value = (Action)p.CreateDelegate(typeof (Action))
				};
			}).Where(p => p != null).ToDictionary(p => p.Key, p => p.Value);
		}

		private class KeyAttribute: Attribute{
			public KeyAttribute(string functionKey) {
				FunctionKey = functionKey;
			}

			public string FunctionKey { get; private set; }
		}

	}
}
