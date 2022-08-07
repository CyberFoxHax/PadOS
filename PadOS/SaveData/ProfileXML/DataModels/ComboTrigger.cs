using System.Collections.Generic;
using System.Xml;
using System.Linq;

namespace PadOS.SaveData.ProfileXML
{
    public class ComboTrigger : ITrigger, IParseXML {
        public int Timeout { get; set; }
        public List<ITrigger> Sequence { get; set; }

        public void Parse(ParseProfileXML ctx, XmlNode node) {
            Sequence = node.ChildNodes
                .Cast<XmlNode>()
                .Select(p => ctx.ReflectNode(p))
                .OfType<ITrigger>()
                .ToList();
        }
    }
}