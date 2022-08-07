using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PadOS.Plugins
{

    public static class PluginsLoader {
        public static IEnumerable<string> FindCorrectDll(IEnumerable<string> configNames) {
            configNames = configNames as IList<string> ?? configNames.ToArray();

            var pluginsRoot = Path.Combine(Environment.CurrentDirectory, "Plugins");
            var allDll = Directory.EnumerateDirectories(pluginsRoot)
                .SelectMany(Directory.EnumerateFiles)
                .Where(p => p.EndsWith(".dll"))
                .ToArray();

            var plugins = new Dictionary<string, string>();
            // find all DLL with matching file names
            foreach (var item in configNames) {
                var a = allDll
                    .FirstOrDefault(p => Path.GetFileName(p) == Path.GetFileName(item));

                if (string.IsNullOrEmpty(a) == false)
                    plugins[a] = item;
            }
            // find all DLL with matching file name, but only those with a path
            foreach (var item in configNames) {
                var file = Path.GetFileName(item);
                if (file.Length < item.Length)
                    continue;
                var a = allDll
                    .FirstOrDefault(p => Path.GetFileName(p) == file);

                if (string.IsNullOrEmpty(a) == false)
                    plugins[a] = item;
            }
            // absolute path
            foreach (var item in configNames) {
                if (File.Exists(item))
                    plugins[item] = item;
            }

            return plugins.Keys;
        }

        public static Plugin<T> Load<T>(string file) {
            var pluginType = typeof(T);
            Assembly assembly;
            try {
                assembly = Assembly.LoadFrom(file);
            }
            catch (Exception) {
                return null;
            }
            foreach (var type in assembly.ExportedTypes)
                if (pluginType.IsAssignableFrom(type))
                    return new Plugin<T> {
                        File = file,
                        Class = type
                    };
            return null;
        }
        public static IEnumerable<Plugin> LoadAll<T>() where T:class {
            if (Directory.Exists("Plugins") == false)
                yield break;

            var pluginType = typeof(T);
            var pluginsDir = Path.Combine(Environment.CurrentDirectory, "Plugins");
            foreach (var pluginDir in Directory.EnumerateDirectories(pluginsDir))
                foreach (var file in Directory.EnumerateFiles(pluginDir)) {
                    Assembly assembly;
                    try {
                        assembly = Assembly.LoadFrom(file);
                    }
                    catch (Exception) {
                        continue;
                    }
                    foreach (var type in assembly.ExportedTypes)
                        if (pluginType.IsAssignableFrom(type))
                            yield return new Plugin {
                                File = file,
                                Class = type
                            };
                }
        }
    }
}
