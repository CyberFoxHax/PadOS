using System;
using System.Collections.Generic;
using System.Linq;

namespace PadOS.ProfileSwitcher
{
    public class ProfileManager {
        private class PluginInstance {
            public Plugins.Plugin Plugin { get; set; }
            public InputSimulatorPlugin Instance { get; set; }
        }

        private static SaveData.ProfileXML.ApplicationAssociation[] _profileMappings;

        public void UpdateProfiles(IEnumerable<SaveData.ProfileXML.ApplicationAssociation> newMappings) {
            _profileMappings = newMappings as SaveData.ProfileXML.ApplicationAssociation[] ?? newMappings.ToArray();

            if (_currentPlugin == null) {
                var a = _profileMappings.FirstOrDefault(p => p.Executable == null);
                var b = _plugins.FirstOrDefault(p => p.Plugin.File.Contains(a.DllName));
                _currentPlugin = b;
            }
        }

        public ProfileManager() {
            _plugins = Plugins.PluginsLoader.LoadAll<InputSimulatorPlugin>()
                .Select(p=>new PluginInstance {
                    Plugin = p,
                    Instance = (InputSimulatorPlugin)Activator.CreateInstance(p.Class)
                })
                .ToArray();

            var tracker = new BackgroundTracker();
            tracker.Enabled = true;
            tracker.ProcessChanged += Tracker_ProcessChanged;
        }

        private IEnumerable<PluginInstance> _plugins;
        private PluginInstance _currentPlugin;

        private bool _enabled;
        public bool ProfileEnabled {
            get { return _enabled; }
            set {
                if (_enabled != value)
                    _currentPlugin.Instance.Enabled = value;
                _enabled = value;
            }
        }


        private void Tracker_ProcessChanged(string oldProcess, string newProcess) {
            var dictMatch = _profileMappings.FirstOrDefault(p => p.Executable == newProcess);
            if (dictMatch == null)
                dictMatch = _profileMappings.FirstOrDefault(p => p.Executable == null);
            var plugin = _plugins.FirstOrDefault(p => p.Plugin.File.Contains(dictMatch.DllName));

            if (plugin == _currentPlugin)
                return;

            if (_currentPlugin != null)
                _currentPlugin.Instance.Enabled = false;

            plugin.Instance.Enabled = true;
            _currentPlugin = plugin;
        }
    }
}
