using System;
using System.Collections.Generic;

namespace PadOS.SaveData.ProfileXML
{
    public class Profile {
        public string Name { get; set; }
        public List<Variable> Variables { get; set; } = new List<Variable>();
        public List<Mapping> Mappings { get; set; } = new List<Mapping>();
        public List<Plugin> Plugins { get; set; } = new List<Plugin>();
    }
}