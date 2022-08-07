using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadOS.SaveData.Models
{
    public class Profile {
        public Int64 Id { get; internal set; }
        public string Name { get; internal set; }
        public string XML { get; internal set; }

        [Newtonsoft.Json.JsonIgnore]
        public ProfileXML.Profile ProfileXML;
    }
}
