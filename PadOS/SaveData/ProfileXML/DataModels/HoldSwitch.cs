using System.Collections.Generic;
using System.Xml;

namespace PadOS.SaveData.ProfileXML
{
    public class HoldSwitch : ITrigger, IParseXML {
        public List<HSTrigger> Triggers { get; set; }
        public List<HSAction> Actions { get; set; }

        public class HSTrigger {
            public ITrigger Owner { get; set; }
        }

        public class HSAction {
            public IAction Owner { get; set; }
            public float Timeout;
        }

        public void Parse(ParseProfileXML ctx, XmlNode node) {
            Triggers = new List<HSTrigger>();
            Actions = new List<HSAction>();
            foreach (XmlNode child in node.ChildNodes) {
                var data = ctx.ReflectNode(child) as ITrigger;
                if (data == null)
                    continue;

                Triggers.Add(new HSTrigger {
                    Owner = data
                });
            }
        }

        public void ParseAction(ParseProfileXML ctx, XmlNode node, IAction action) {
            var value = node.Attributes[$"{nameof(HoldSwitch)}.{nameof(HSAction.Timeout)}"];
            if (value == null)
                return;
            Actions.Add(new HSAction { 
                Owner = action,
                Timeout = float.Parse(value.Value)
            });
        }
    }
}