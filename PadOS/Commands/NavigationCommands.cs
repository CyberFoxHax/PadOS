using System.Windows.Input;
using PadOS.Navigation;

namespace PadOS.Commands{
	public static class NavigationCommands{
		static NavigationCommands(){
            EditProfileAssociationsCommand = new RelayCommand(EditProfileAssociations);
			EditCircleDialCommand = new RelayCommand(EditCircleDial);
			EditInputSimulatorCommand = new RelayCommand(EditInputSimulatorProfiles);
		}

		public static ICommand EditProfileAssociationsCommand { get; private set; }
		public static ICommand EditCircleDialCommand { get; private set; }
        public static ICommand EditInputSimulatorCommand { get; private set; }

		private static void EditCircleDial 			    (){ Navigator.OpenWindow<Views.CircleDialEditor.CircleDialEditor>(); }
		private static void EditInputSimulatorProfiles  (){ Navigator.OpenWindow<Views.InputSimulationEditor.InputSimulationMainWindow>(); }
        private static void EditProfileAssociations     (){ Navigator.OpenWindow<Views.ProfileAssociationEditor.ProfileAssociationEditor>(); }

    }
}