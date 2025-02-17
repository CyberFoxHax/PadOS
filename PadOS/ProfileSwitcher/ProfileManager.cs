using System;
using System.Collections.Generic;
using System.Linq;

namespace PadOS.ProfileSwitcher
{
    public class ProfileManager {
        public void Init() {
            _profiles = new Dictionary<Int64, ProfileExecution.Executor>();

            using (var saveData = new SaveData.SaveData()) {
                foreach (var item in saveData.Profiles) {
                    if (string.IsNullOrEmpty(item.XML))
                        continue;
                    item.ProfileXML = SaveData.ProfileXML.ParseProfileXML.LoadFile(item.XML).Parse();
                    var input = Input.GamePadInput.GamePadInput.StaticInputInstance;
                    var executor = new ProfileExecution.Executor(item, input);
                    executor.Init();
                    _profiles[item.Id] = executor;
                }
                _profileMappings = saveData.ProfileAssociations.ToArray();
                saveData.ProfileAssociations.OnUnderlyingDataChanged += OnSavedProfilesChanged;
            }
            CurrentProfile = _profiles.First().Value;


            _tracker = new BackgroundTracker();
            _tracker.Enabled = true;
            _tracker.ProcessChanged += Tracker_ProcessChanged;
        }

        private void OnSavedProfilesChanged(SaveData.JsonDatastore.JsonTable obj) {
            _profileMappings = obj.Cast<SaveData.Models.ProfileAssociation>().ToArray();
            Tracker_ProcessChanged(null, _currentProccess);
        }

        private static SaveData.Models.ProfileAssociation[] _profileMappings;
        private BackgroundTracker _tracker;
        private Dictionary<Int64, ProfileExecution.Executor> _profiles;

        public ProfileExecution.Executor CurrentProfile { get; private set; }

        private bool _profileEnabled;
        public bool ProfileEnabled {
            get { return _profileEnabled; }
            set {
                if (_profileEnabled == value)
                    return;
                _profileEnabled = value;
                _tracker.Enabled = value;
                if (CurrentProfile != null)
                    CurrentProfile.Enabled = value;
            }
        }

        private string _currentProccess;
        //private string _currentWindow; // TBA

        private async void Tracker_ProcessChanged(string oldProcess, string newProcess) {
            _currentProccess = newProcess;
            var processName = System.IO.Path.GetFileName(newProcess);
            var profileMatch = _profileMappings.FirstOrDefault(p => p.Executable == processName);
            if (profileMatch == null)
                profileMatch = _profileMappings.FirstOrDefault(p => p.Executable == null);

            var newProfile = _profiles[profileMatch.Profile.Id];
            if (newProfile == CurrentProfile) {
                Console.WriteLine("[ProfileManager/Tracker_ProcessChanged] Process changed to: " + processName + ". Profile change not needed");
                return;
            }
            Console.WriteLine("[ProfileManager/Tracker_ProcessChanged] Profile changing");
            _tracker.Enabled = false;
            await CurrentProfile.AwaitAllKeysUp();
            Console.WriteLine("[ProfileManager/Tracker_ProcessChanged] Process changed to: " + processName + ". Profile changed to \"" + profileMatch.Profile.Name + "\"");
            CurrentProfile.Enabled = false;
            CurrentProfile = newProfile;
            CurrentProfile.Enabled = true;
            _tracker.Enabled = true;
        }
    }
}
