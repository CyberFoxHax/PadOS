using System.Collections.Generic;
using PadOS.ViewModels.FunctionButtons;

namespace PadOS.SaveData {

	public class MainPanelData : ISaveData<MainPanelData> {
		public List<PanelItem> Items { get; set; }

		public class PanelItem {
			public string ImageUri { get; set; }
			public string Key { get; set; }
			public string Title { get; set; }
			public int Position { get; set; }
			public FunctionType FunctionType { get; set; }
		}

		public MainPanelData GetDefault() {
			return new MainPanelData {
				Items = new List<PanelItem>{
					new PanelItem{ Position = 4, Key = "OpenSettings", ImageUri = "Icons/cogs.png",	Title = "Settings", FunctionType = FunctionType.PadOsInternal},
					new PanelItem{ Position = 6, Key = "OpenOsk"	 , ImageUri = "Icons/osk.png",	Title = "OSK",		FunctionType = FunctionType.PadOsInternal}
				}
			};
		}
	}

}
