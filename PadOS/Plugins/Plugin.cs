using System;

namespace PadOS.Plugins
{
    public class Plugin {
        public string File { get; set; }
        public Type Class { get; set; }

        public object CreateInstance() {
            var instance = Activator.CreateInstance(Class, new object[0]);
            return instance;
        }
    }

    public class Plugin<T> : Plugin {
        public new T CreateInstance() {
            return (T)base.CreateInstance();
        }
    }
}
