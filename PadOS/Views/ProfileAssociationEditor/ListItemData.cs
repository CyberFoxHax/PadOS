using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadOS.SaveData.Models;

namespace PadOS.Views.ProfileAssociationEditor
{
    public class ListItemData {
        public string Title { get; set; }
        public ProfileAssociation Data { get; internal set; }
    }
}
