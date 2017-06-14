using System;

namespace PadOS.SaveData.Models {

	public class MainPanelData {
		public Int64 Id { get; set; }
		public int Position { get; set; }

		public virtual FunctionButton FunctionButton { get; set; }
	}

}
