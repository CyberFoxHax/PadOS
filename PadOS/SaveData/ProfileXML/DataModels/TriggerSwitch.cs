using System.Collections.Generic;
using System.Xml;
using System.Linq;

namespace PadOS.SaveData.ProfileXML {
    public class TriggerSwitch : ITrigger, IParseXML{
        public List<ITrigger> Triggers { get; set; }

        public void Parse(ParseProfileXML ctx, XmlNode node) {
            Triggers = node.ChildNodes
                .Cast<XmlNode>()
                .Select(p => ctx.ReflectNode(p))
                .OfType<ITrigger>()
                .ToList();
        }
    }
}