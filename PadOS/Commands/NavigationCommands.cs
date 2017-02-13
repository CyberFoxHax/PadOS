using System.Windows.Input;

namespace PadOS.Commands{
	public static class NavigationCommands{
		static NavigationCommands(){
			ChangePlayersCommand = new RelayCommand(ChangePlayers);
			EditGamePadsCommand = new RelayCommand(EditGamePads);
			EditMainPanelCommand = new RelayCommand(EditMainPanel);
			EditKeyboardMappingsCommand = new RelayCommand(EditKeyboardMappings);
			EditKeyboardProfilesCommand = new RelayCommand(EditKeyboardProfiles);
			EditGamePadCalibrationCommand = new RelayCommand(EditGamePadCalibration);
			EditMainPanelProfileCommand = new RelayCommand(EditMainPanelProfile);
		}

		public static ICommand ChangePlayersCommand { get; private set; }
		public static ICommand EditGamePadsCommand { get; private set; }
		public static ICommand EditMainPanelCommand { get; private set; }
		public static ICommand EditKeyboardMappingsCommand { get; private set; }
		public static ICommand EditKeyboardProfilesCommand { get; private set; }
		public static ICommand EditGamePadCalibrationCommand { get; private set; }
		public static ICommand EditMainPanelProfileCommand { get; private set; }

		private static void ChangePlayers			(){ new Views.ConfigurePlayers.PlayerEditor().Show(); } 
		private static void EditGamePads 			(){}
		private static void EditMainPanel 			(){}
		private static void EditKeyboardMappings	(){}
		private static void EditKeyboardProfiles	(){}
		private static void EditGamePadCalibration	(){}
		private static void EditMainPanelProfile	(){}
	}
}