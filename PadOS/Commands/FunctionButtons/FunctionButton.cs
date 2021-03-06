﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PadOS.Commands.FunctionButtons {
	public class FunctionButton {
		public Uri ImageUri { get; set; }
		public string Title { get; set; }
		public string Identifier { get; set; }
		public FunctionType FunctionType { get; set; }

		public void Exec(){
			switch (FunctionType){
				case FunctionType.None:
					break;
				case FunctionType.PadOsInternal:
					var f = PadOsInternalFunctions.GetFunction(Identifier);
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
