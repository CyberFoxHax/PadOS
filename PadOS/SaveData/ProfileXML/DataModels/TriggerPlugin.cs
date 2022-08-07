using System.Collections.Generic;
using System.Xml;

namespace PadOS.SaveData.ProfileXML
{
    public class TriggerPlugin : ITrigger, IParseXML {
        public string Filename { get; set; }
        public List<Param> Parameters { get; set; }

        public void Parse(ParseProfileXML ctx, XmlNode node) {
            Parameters = new List<Param>();
            foreach (XmlNode item in node.ChildNodes) {
                var p = ctx.ReflectNode<Param>(item);
                if(p != null)
                    Parameters.Add(p);
            }
        }
    }
}