using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadOS
{
    public interface InputSimulatorPlugin {
        string Key { get; }
        bool Enabled { get; set; }
    }
}
