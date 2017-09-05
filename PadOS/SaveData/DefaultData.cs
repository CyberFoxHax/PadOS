using PadOS.Commands.FunctionButtons;
using PadOS.SaveData.Models;
using FunctionButton = PadOS.SaveData.Models.FunctionButton;

namespace PadOS.SaveData {
	public static class DefaultData{
		public static FunctionButton[] FunctionButtons = {
			new FunctionButton{
				Parameter = "Empty",
				Title = "Empty",
				FunctionType = FunctionType.PadOsInternal
			},
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
			new MainPanelData{Position = 4, FunctionButton = FunctionButtons[1]},
			new MainPanelData{Position = 6, FunctionButton = FunctionButtons[2]}
		};
	}
}
