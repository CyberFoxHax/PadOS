using System.Collections.Generic;
using System.Xml;

namespace PadOS.SaveData.ProfileXML
{
    public class ActionPlugin : IAction, IParseXML {
        public string Filename { get; set; }
        public List<Param> Parameters { get; set; }

        public void Parse(ParseProfileXML ctx, XmlNode node) {
            Parameters = new List<Param>();
            foreach (XmlNode param in node.ChildNodes){
                if (param.Name != nameof(Param))
                    continue;
                var paramInstance = (Param)ctx.ReflectNode(param, typeof(Param));
                Parameters.Add(paramInstance);
            }
        }
    }
}