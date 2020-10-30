using System;
using System.Collections.Generic;

namespace PadOS.SaveData.ProfileXML
{
    public class Profile {
        public string Name { get; set; }
        public List<Variable> Variables { get; set; }
        public List<Mapping> Mappings { get; set; }
    }
}