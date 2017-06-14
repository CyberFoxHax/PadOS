using System.Collections.Generic;
using PadOS.SaveData.Models;
using PadOS.ViewModels.FunctionButtons;
using FunctionButton = PadOS.SaveData.Models.FunctionButton;

namespace PadOS.SaveData {
	public static class DefaultData{
		public static FunctionButton[] FunctionButtons ={
			new FunctionButton{
				Parameter = "OpenSettings",
				ImageUri = "Icons/cogs.png",
				Title = "Settings",
				FunctionType = FunctionType.PadOsInternal
			},
			new FunctionButton{
				Parameter = "OpenOsk",
				ImageUri = "Icons/osk.png",
				Title = "OSK",
				FunctionType = FunctionType.PadOsInternal
			}
		};

		public static MainPanelData[] MainPanelData = {
			new MainPanelData { Position = 4, FunctionButton = FunctionButtons[0] },
			new MainPanelData { Position = 6, FunctionButton = FunctionButtons[1] }
		};
	}
}
