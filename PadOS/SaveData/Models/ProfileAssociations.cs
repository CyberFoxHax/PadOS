using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadOS.SaveData.Models {
    public class ProfileAssociation {
        public Int64 Id { get; set; }
        public string Executable { get; set; }
        public string WindowTitle { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
