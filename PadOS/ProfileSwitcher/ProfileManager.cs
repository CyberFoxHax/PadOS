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
            _tracker.WindowChanged += Tracker_WindowChanged;
        }

        private void OnSavedProfilesChanged(SaveData.JsonDatastore.JsonTable obj) {
            _profileMappings = obj.Cast<SaveData.Models.ProfileAssociation>().ToArray();
            MatchProfile();
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

        private string _currentProcess;
        private string _currentWindow;

        private void Tracker_WindowChanged(string newProcess, string newWindow) {
            _currentProcess = newProcess;
            _currentWindow = newWindow;
            MatchProfile();
        }

        private bool ExecWindowTest(SaveData.Models.ProfileAssociation profile, string exec, string window) {
            var a = "";
            var b = "";
            if (string.IsNullOrEmpty(profile.Executable) == false) {
                a += profile.Executable;
                if(System.IO.Path.IsPathRooted(profile.Executable))
                    b += exec;
                else
                    b += System.IO.Path.GetFileName(exec);
            }
            if (string.IsNullOrEmpty(profile.WindowTitle) == false) {
                if (string.IsNullOrEmpty(a) == false)
                    a += "+";
                a += profile.WindowTitle;

                if (string.IsNullOrEmpty(b) == false)
                    b += "+";
                b += window;
            }
            if(string.IsNullOrEmpty(a) && string.IsNullOrEmpty(b))
                return false;
            return a == b;
        }

        private async void MatchProfile() {
            var profileMatch = _profileMappings.FirstOrDefault(p => ExecWindowTest(p, _currentProcess, _currentWindow));
            if (profileMatch == null) // select default profile
                profileMatch = _profileMappings.FirstOrDefault(p => p.Executable == null && p.WindowTitle == null);

            // Produce a debug string to print to console 0_0
            var matchStr = "";
            if (string.IsNullOrEmpty(_currentProcess) == false)
                matchStr += System.IO.Path.GetFileName(_currentProcess);
            if (string.IsNullOrEmpty(_currentWindow) == false) {
                if (string.IsNullOrEmpty(_currentProcess) == false)
                    matchStr += "+";
                matchStr += _currentWindow;
            }

            var newProfile = _profiles[profileMatch.Profile.Id];
            if (newProfile == CurrentProfile) {
                Console.WriteLine("[ProfileManager/MatchProfile] Window changed to: \"" + matchStr + "\". Profile change not needed");
                return;
            }
            Console.WriteLine("[ProfileManager/MatchProfile] Profile changing");
            _tracker.Enabled = false;
            await CurrentProfile.AwaitAllKeysUp();
            Console.WriteLine("[ProfileManager/MatchProfile] Window changed to: " + matchStr + ". Profile changed to \"" + profileMatch.Profile.Name + "\"");
            CurrentProfile.Enabled = false;
            CurrentProfile = newProfile;
            CurrentProfile.Enabled = true;
            _tracker.Enabled = true;
        }
    }
}
