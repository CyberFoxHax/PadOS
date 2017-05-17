using System.Collections.Generic;

namespace PadOS.SaveData {

	public class MainPanelData : ISaveData<MainPanelData> {
		public List<PanelItem> Items { get; set; }

		public class PanelItem {
			public string ImageUri { get; set; }
			public string Key { get; set; }
			public int Position { get; set; }
		}

		public MainPanelData GetDefault() {
			return new MainPanelData {
				Items = new List<PanelItem>{
					new PanelItem{ Position = 4, Key = "settings", ImageUri = "Icons/cogs.png"	},
					new PanelItem{ Position = 6, Key = "osk"	 , ImageUri = "Icons/osk.png"	}
				}
			};
		}
	}

}
