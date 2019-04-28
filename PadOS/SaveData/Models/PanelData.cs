using System;

namespace PadOS.SaveData.Models {
	public class PanelButton {
		public Int64 Id { get; set; }
		public int Position { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual Function Function { get; set; }
	}
}
