using System.Collections.Generic;
using System.Xml;

namespace PadOS.SaveData.ProfileXML
{
    public class KeyboardAction : IAction, IParseXML  {
        public List<string> Buttons { get; set; }

        public void Parse(ParseProfileXML ctx, XmlNode node) {
            var button = node.Attributes["Button"].Value;
            Buttons = new List<string>();
            foreach (var item in button.Split('+'))
                Buttons.Add(item);
        }
    }
}