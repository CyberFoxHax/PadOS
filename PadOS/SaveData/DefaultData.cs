using PadOS.Commands.FunctionButtons;
using PadOS.SaveData.Models;
using Function = PadOS.SaveData.Models.Function;

namespace PadOS.SaveData {
	public static class DefaultData{
        public static void InsertData(SaveData ctx) {
            ctx.Functions.AddRange(Functions);
            ctx.PanelButtons.AddRange(PanelButtons);
            ctx.Profiles.AddRange(Profiles);
            ctx.ProfileAssociations.AddRange(ProfileAssociations);
            ctx.SaveChanges();
        }

        public static readonly Function EmptyFunction = new Function {
            Id = 1,
            Parameter = "Empty",
            Title = "Empty",
            FunctionType = FunctionType.PadOsInternal,
        };

        public static readonly Profile AllProfile = new Profile {
            Id = 1,
            Name = "All",
            XML = null
        };

        public static readonly Profile DefaultProfile = new Profile {
            Id = 2,
            Name = "Default",
            XML = "Default.xml"
        };


        public static Function[] Functions = {
			EmptyFunction,
			new Function{
                Id = 2,
                Parameter = "OpenSettings",
				ImageUrl = "Icons/cogs.png",
				Title = "Settings",
				FunctionType = FunctionType.PadOsInternal
			},
			new Function{
                Id = 3,
                Parameter = "OpenOsk",
				ImageUrl = "Icons/osk.png",
				Title = "OSK",
				FunctionType = FunctionType.PadOsInternal
			}
		};

        public static Profile[] Profiles = {
            AllProfile,
            DefaultProfile,
            new Profile {
                Id = 3,
                Name = "Supported Game",
                XML = "Game.xml"
            },
            new Profile {
                Id = 4,
                Name = "VLC",
                XML = "VLC.xml"
            },
            new Profile {
                Id = 5,
                Name = "Dolphin",
                XML = "Dolphin.xml"
            },
            new Profile {
                Id = 6,
                Name = "PCSX2",
                XML = "PCSX2.xml"
            },
        };

		public static PanelButton[] PanelButtons = {
			new PanelButton {Position = 4, Function = Functions[1], Profile = Profiles[0]},
			new PanelButton {Position = 6, Function = Functions[2], Profile = Profiles[0]}
		};

        public static readonly ProfileAssociation[] ProfileAssociations = new ProfileAssociation[] {
            new ProfileAssociation{
               Executable = null,
               Profile = DefaultProfile,
               WindowTitle = null,
            },
            new ProfileAssociation{
               Executable = "vlc.exe",
               Profile = Profiles[4-1],
               WindowTitle = null,
            },
            new ProfileAssociation{
               Executable = "dolphin.exe",
               Profile = Profiles[5-1],
               WindowTitle = null,
            },
            new ProfileAssociation{
               Executable = "pcsx2.exe",
               Profile = Profiles[6-1],
               WindowTitle = null,
            },
        };

    }
}
