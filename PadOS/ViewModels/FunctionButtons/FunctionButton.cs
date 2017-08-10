using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PadOS.ViewModels.FunctionButtons {
	public class FunctionButton {
		public Uri ImageUri { get; set; }
		public string Title { get; set; }
		public string Key { get; set; }
		public FunctionType FunctionType { get; set; }

		public virtual void Exec(){
			switch (FunctionType){
				case FunctionType.None:
					break;
				case FunctionType.PadOsInternal:
					var f = PadOsInternalFunctions.GetFunction(Key);
					if (f != null)
						f();
					else if(Debugger.IsAttached)
						throw new KeyNotFoundException();
					break;
				case FunctionType.SimulateInput:
					break;
				case FunctionType.MediaApi:
					break;
				case FunctionType.ExecuteProgram:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
