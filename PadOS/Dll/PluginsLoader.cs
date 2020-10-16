using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PadOS.Dll
{
    public static class PluginsLoader {
        public static IEnumerable<Type> LoadAll<T>() where T:class {
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
                            yield return type;
                }
        }
    }
}
