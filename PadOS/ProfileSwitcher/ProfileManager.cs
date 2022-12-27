using System;
using System.Collections.Generic;
using System.Linq;

namespace PadOS.ProfileSwitcher
{
    public class ProfileManager {
        public void Init(SaveData.SaveData saveData) {
            _profiles = new Dictionary<SaveData.Models.Profile, ProfileExecution.Executor>();
            foreach (var item in saveData.Profiles) {
                if (string.IsNullOrEmpty(item.XML))
                    continue;
                item.ProfileXML = SaveData.ProfileXML.ParseProfileXML.LoadFile(item.XML).Parse();
                var input = Input.GamePadInput.GamePadInput.StaticInputInstance;
                var executor = new ProfileExecution.Executor(item.ProfileXML, input);
                executor.Init();
                _profiles[item] = executor;
            }
            _currentProfile = _profiles.First().Value;
            _profileMappings = saveData.ProfileAssociations.ToArray();

            _tracker = new BackgroundTracker();
            _tracker.Enabled = true;
            _tracker.ProcessChanged += Tracker_ProcessChanged;
        }

        private static SaveData.Models.ProfileAssociation[] _profileMappings;
        private BackgroundTracker _tracker;
        private Dictionary<SaveData.Models.Profile, ProfileExecution.Executor> _profiles;
        private ProfileExecution.Executor _currentProfile;

        private bool _profileEnabled;
        public bool ProfileEnabled {
            get { return _profileEnabled; }
            set {
                if (_profileEnabled == value)
                    return;
                _profileEnabled = value;
                _tracker.Enabled = value;
                if (_currentProfile != null)
                    _currentProfile.Enabled = value;
            }
        }

        private async void Tracker_ProcessChanged(string oldProcess, string newProcess) {
            var processName = System.IO.Path.GetFileName(newProcess);
            var profileMatch = _profileMappings.FirstOrDefault(p => p.Executable == processName);
            if (profileMatch == null)
                profileMatch = _profileMappings.FirstOrDefault(p => p.Executable == null);

            var newProfile = _profiles[profileMatch.Profile];
            if (newProfile == _currentProfile) {
                Console.WriteLine("Process changed to: " + processName + ". Profile change not needed");
                return;
            }
            Console.WriteLine("Process changed to: " + processName + ". Profile changed to \"" + profileMatch.Profile.Name + "\"");
            _tracker.Enabled = false;
            await _currentProfile.AwaitAllKeysUp();
            Console.WriteLine("Wait completed");
            _currentProfile.Enabled = false;
            _currentProfile = newProfile;
            _currentProfile.Enabled = true;
            _tracker.Enabled = true;
        }
    }
}
