using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PadOS.Plugins
{

    public static class PluginsLoader {

        /// <summary>
        /// Looks for the dllfile names, returns an array of files should multiple candidates exist.
        /// Takes a list of paths forcing you to cache the result, and not run repeated queries on the filesystem.
        /// </summary>
        /// <param name="pluginPaths">list of paths, relative or absolute</param>
        /// <returns></returns>
        public static IEnumerable<string> FindCorrectDll(IEnumerable<string> pluginPaths) {
            pluginPaths = pluginPaths as IList<string> ?? pluginPaths.ToArray();

            var pluginsRoot = Path.Combine(Environment.CurrentDirectory, "Plugins");
            if (Directory.Exists(pluginsRoot) == false)
                Directory.CreateDirectory(pluginsRoot);
            var allDll = Directory.EnumerateDirectories(pluginsRoot)
                .SelectMany(Directory.EnumerateFiles)
                .Where(p => p.EndsWith(".dll"))
                .ToArray();

            var plugins = new Dictionary<string, string>();
            // find all DLL with matching file names
            foreach (var item in pluginPaths) {
                var a = allDll
                    .FirstOrDefault(p => Path.GetFileName(p) == Path.GetFileName(item));

                if (string.IsNullOrEmpty(a) == false)
                    plugins[item] = a;
            }
            // find all DLL with matching file name, but only those with a path
            foreach (var item in pluginPaths) {
                var file = Path.GetFileName(item);
                if (file.Length < item.Length)
                    continue;
                var a = allDll
                    .FirstOrDefault(p => Path.GetFileName(p) == file);

                if (string.IsNullOrEmpty(a) == false)
                    plugins[item] = a;
            }
            // absolute path
            foreach (var item in pluginPaths) {
                if (File.Exists(item))
                    plugins[item] = item;
            }

            foreach (var p in pluginPaths) {
                if (plugins.ContainsKey(p) == false)
                    Console.Error.WriteLine("[PluginsLoader/FindCorrectDll] Plugin not found: \"" + p + "\"");
            }
            return plugins.Values;
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
