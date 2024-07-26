using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadOS
{
    public interface IInputSimulatorPlugin {
        /// <summary>
        /// Custom name you can reference in your XML. Not implemented!
        /// </summary>
        string Key { get; }

        /// <summary>Getter/Setter to enable and disable plugin. Plugins need to implement this correctly</summary>
        bool Enabled { get; set; }
    }
}
