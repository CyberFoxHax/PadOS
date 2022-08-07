using System.Collections.Generic;

namespace PadOS.SaveData.ProfileXML
{
    public class Mapping {
        public List<ITrigger> Triggers { get; set; } = new List<ITrigger>();
        public List<IAction> Actions { get; set; } = new List<IAction>();
    }
}