using System.Collections.Generic;

namespace PadOS.SaveData {
	public static class MainPanel{
		private const string Filename = "MainPanel.json";

		public static MainPanelModel Data { get; set; }

		public static void Save() {
			SaveDataUtils.SaveData(Filename, Data);
		}

		public static MainPanelModel Load(){
			return Data = SaveDataUtils.LoadData<MainPanelModel>(Filename) ?? MainPanelModel.GetDefault();
		}

		public class MainPanelModel {
			public List<PanelItem> Items { get; set; }

			public class PanelItem {
				public string ImageUri { get; set; }
				public string Key { get; set; }
				public int Position { get; set; }
			}

			public static MainPanelModel GetDefault() {
				return new MainPanelModel {
					Items = new List<PanelItem>{
						new PanelItem{Position = 4, Key = "settings", ImageUri = "Icons/cogs.png"},
						new PanelItem{Position = 6, Key = "osk"		, ImageUri = "Icons/osk.png"}
					}
				};
			}
		}
	}
}
