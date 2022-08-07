using System.Collections.Generic;
using System.Xml;

namespace PadOS.SaveData.ProfileXML
{
    public class MouseAction : IAction, IParseXML {
        public string Delay { get; set; }
        public List<Click> Buttons { get; set; }

        public class Click {
            public int Button { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
        }

        public void Parse(ParseProfileXML ctx, XmlNode node) {
            Buttons = new List<Click>();
            foreach (XmlNode item in node.ChildNodes) {
                if (item.Name == nameof(Click)) {
                    var button = item.Attributes[nameof(Click.Button)];
                    var x = item.Attributes[nameof(Click.X)];
                    var y = item.Attributes[nameof(Click.Y)];

                    var click = new Click();
                    if(x!=null) click.X = int.Parse(x.Value);
                    if(y!=null) click.Y = int.Parse(y.Value);
                    if (button!=null) {
                        switch (button.Value) {
                            case "Left":
                            case "1":
                                click.Button = 1;
                                break;
                            case "Middle":
                            case "Wheel":
                            case "2":
                                click.Button = 2;
                                break;
                            case "Right":
                            case "3":
                                click.Button = 3;
                                break;
                            case "Forward":
                            case "4":
                                click.Button = 4;
                                break;
                            case "Back":
                            case "5":
                                click.Button = 5;
                                break;
                        }
                    }
                    Buttons.Add(click);
                }
            }
        }
    }
}