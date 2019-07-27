using System;

namespace PadOS.SaveData.Models {
	public class PanelButton {
		public Int64 Id { get; set; }
		public int Position { get; set; }
        public virtual Lazy<Profile> Profile { get; set; }
        public virtual Lazy<Function> Function { get; set; }
	}
}
