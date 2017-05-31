using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadOS.ViewModels.FunctionButtons;

namespace PadOS.SaveData.Models {
	public class FunctionButtons : ISaveData<FunctionButtons>{
		public List<JsonFunctionButton> Items { get; set; }

		public FunctionButtons GetDefault(){
			return new FunctionButtons{
				Items = new List<JsonFunctionButton>{
					new JsonFunctionButton{ Id = 1, Parameter = "OpenSettings", ImageUri = "Icons/cogs.png",	Title = "Settings", FunctionType = FunctionType.PadOsInternal},
					new JsonFunctionButton{ Id = 2, Parameter = "OpenOsk"		, ImageUri = "Icons/osk.png",	Title = "OSK",		FunctionType = FunctionType.PadOsInternal}
				}
			};
		}
	}

	public class JsonFunctionButton {
		public int Id { get; set; }
		public string Title { get; set; }
		public FunctionType FunctionType { get; set; }
		public string Parameter { get; set; }
		public string ImageUri { get; set; }
	}
}
