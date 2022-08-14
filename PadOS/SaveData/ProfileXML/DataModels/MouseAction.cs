using System.Collections.Generic;
using System.Xml;

namespace PadOS.SaveData.ProfileXML
{
    public class MouseAction : IAction, IParseXML {
        public Vector2 Position { get; set; }
        public EButton Button { get; set; }
        public EAxis Scroll { get; set; }
        //public EAxis Move { get; set; } // todo later, not important for V1.0
        public float Speed { get; set; }

        public enum EAxis { Undefined, X, Y }
        public enum EButton {
            Undefined = 0,
            Left = 1,
            Middle = 2,
            Scroll = 2,
            Right = 3,
            Forward = 4,
            Back = 5
        }

        public enum EUnit {
            Pixels,
            Percentage
        }

        public struct Vector2 {
            public float X;
            public float Y;
            public EUnit XUnit;
            public EUnit YUnit;
            public bool IsNaN() => float.IsNaN(X) || float.IsNaN(Y);
        }

        public void Parse(ParseProfileXML ctx, XmlNode node) {
            var attr = node.Attributes[nameof(Position)];
            if (attr != null) {
                var split = attr.Value.Split(' ');
                var vec2 = new Vector2();
                if (split.Length == 1) {
                    split = new[] { split[0], split[0] };
                }
                if (split[0].EndsWith("%")) {
                    split[0] = split[0].Replace("%", "");
                    vec2.XUnit = EUnit.Percentage;
                }
                if (split[1].EndsWith("%")) {
                    split[1] = split[1].Replace("%", "");
                    vec2.YUnit = EUnit.Percentage;
                }
                vec2.X = float.Parse(split[0]);
                vec2.Y = float.Parse(split[1]);
                Position = vec2;
            }
            else
                Position = new Vector2 { X=float.NaN, Y=float.NaN };
        }
    }
}