using System.Collections.Generic;
using System.Xml;

namespace PadOS.SaveData.ProfileXML
{
    public class HoldSwitch : ITrigger, IParseXML {
        public List<ChildValues> Buttons { get; set; }

        public class ChildValues {
            public ITrigger Owner { get; set; }
            public float Timeout;
        }

        public void Parse(ParseProfileXML ctx, XmlNode node) {
            Buttons = new List<ChildValues>();
            foreach (XmlNode child in node.ChildNodes) {
                var data = ctx.ReflectNode(child) as ButtonTrigger;
                if (data == null)
                    continue;

                var attr = child.Attributes[nameof(HoldSwitch) + "." + nameof(ChildValues.Timeout)];
                if (attr != null) {
                    var v = new ChildValues {
                        Owner = data,
                        Timeout = float.Parse(attr.Value)
                    };
                    Buttons.Add(v);
                }
            }
        }
    }
}