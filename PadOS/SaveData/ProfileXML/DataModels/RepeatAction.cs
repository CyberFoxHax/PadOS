using System.Collections.Generic;
using System.Xml;
using System.Linq;

namespace PadOS.SaveData.ProfileXML
{
    public class RepeatAction : IAction, IParseXML {
        public int Delay { get; set; } = 1000;
        public int Interval { get; set; } = 100;
        public List<IAction> Actions;

        public void Parse(ParseProfileXML ctx, XmlNode node) {
            Actions = new List<IAction>();
            foreach (XmlNode item in node.ChildNodes) {
                var child = ctx.ReflectNode(item) as IAction;
                if (child == null)
                    continue; // invalid node
                Actions.Add(child);
            }
        }
    }
}