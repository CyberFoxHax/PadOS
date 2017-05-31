using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace PadOS.SaveData.Models {

	public class MainPanelData : ISaveData<MainPanelData> {
		public List<PanelItem> Items { get; set; }

		public class PanelItem {
			public int Position { get; set; }
			public int FunctionId { get; set; }

			private JsonFunctionButton _button;
			[JsonIgnore]
			public JsonFunctionButton JsonFunction{
				get { return _button ?? (_button = SaveData.Load<FunctionButtons>().Items.First(p=>p.Id == FunctionId)); }
			}
		}

		public MainPanelData GetDefault() {
			return new MainPanelData {
				Items = new List<PanelItem>{
					new PanelItem{ Position = 4, FunctionId = 1 },
					new PanelItem{ Position = 6, FunctionId = 2 }
				}
			};
		}
	}

}
