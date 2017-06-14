using System;
using PadOS.ViewModels.FunctionButtons;

namespace PadOS.SaveData.Models {
	public class FunctionButton {
		public Int64 Id { get; set; }
		public string Title { get; set; }
		public FunctionType FunctionType { get; set; }
		public string Parameter { get; set; }
		public string ImageUri { get; set; }
	}
}
