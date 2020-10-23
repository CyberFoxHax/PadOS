using System;
using System.Collections.Generic;
using System.Linq;

namespace PadOS.ProfileSwitcher
{
    public class ProfileManager {
        private class PluginInstance {
            public Dll.Plugin Plugin { get; set; }
            public InputSimulatorPlugin Instance { get; set; }
        }

        private class ProfileMapping {
            public string ProcessName { get; set; }
            public string DllName { get; set; }
        }

        private static ProfileMapping[] _profileMappings = new [] {
            new ProfileMapping{ ProcessName = null, DllName = "PadOS.Plugin.DesktopInput.dll" }
            //new ProfileMapping{ ProcessName = "PadOS", DllName = null }
        };

        public ProfileManager() {
            _plugins = Dll.PluginsLoader.LoadAll<InputSimulatorPlugin>()
                .Select(p=>new PluginInstance {
                    Plugin = p,
                    Instance = (InputSimulatorPlugin)Activator.CreateInstance(p.Class)
                })
                .ToArray();

            var tracker = new BackgroundTracker();
            tracker.Enabled = true;
            tracker.ProcessChanged += Tracker_ProcessChanged;

            var a = _profileMappings.FirstOrDefault(p => p.ProcessName == null);
            var b = _plugins.FirstOrDefault(p => p.Plugin.File.Contains(a.DllName));
            _currentPlugin = b;
        }

        private IEnumerable<PluginInstance> _plugins;
        private PluginInstance _currentPlugin;

        private bool _enabled;
        public bool ProfileEnabled {
            get { return _enabled; }
            set {
                if (_enabled != value) {
                    if (value == true)
                        _currentPlugin.Instance.Load();
                    else
                        _currentPlugin.Instance.Unload();
                }
                _enabled = value;
            }
        }


        private void Tracker_ProcessChanged(string processName) {
            _currentPlugin?.Instance.Unload();

            var dictMatch = _profileMappings.FirstOrDefault(p => p.ProcessName == processName);
            if (dictMatch == null)
                dictMatch = _profileMappings.FirstOrDefault(p => p.ProcessName == null);
            var plugin = _plugins.FirstOrDefault(p => p.Plugin.File.Contains(dictMatch.DllName));

            if (plugin == _currentPlugin)
                return;

            plugin.Instance.Load();
            _currentPlugin = plugin;
        }
    }
}
