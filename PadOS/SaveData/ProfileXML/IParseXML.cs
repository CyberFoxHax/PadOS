using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadOS.SaveData.ProfileXML
{
    interface IParseXML {
        void Parse(ParseProfileXML ctx, System.Xml.XmlNode node);
    }
}
