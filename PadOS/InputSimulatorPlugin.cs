﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadOS
{
    public interface InputSimulatorPlugin {
        void Load();
        void Unload();
    }
}
