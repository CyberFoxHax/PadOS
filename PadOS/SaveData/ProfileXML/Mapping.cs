using System.Collections.Generic;

namespace PadOS.SaveData.ProfileXML
{
    public class Mapping {
        public List<ITrigger> Triggers { get; set; }
        public List<IAction> Actions { get; set; }
        public List<Plugin> Plugins { get; set; }
    }
    

    public class Param {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class ButtonTrigger : ITrigger {
        public string Button { get; set; }
    }

    public class KeyboardAction : IAction  {
        public string Button { get; set; }
    }

    public class RepeatAction : IAction {
        public int Delay { get; set; } = 1000;
    }

    public class HoldSwitch : ITrigger {
        public int Delay { get; set; } = 1000;
        public string Button { get; set; }
    }

    public class Plugin {
        public string Key { get; set; }
        public List<Param> Parameters { get; set; }
    }

    public class TriggerPlugin : ITrigger {
        public string Key { get; set; }
        public List<Param> Parameters { get; set; }
    }

    public class ActionPlugin : IAction {
        public string Key { get; set; }
        public List<Param> Parameters { get; set; }
    }

    public class SequenceTrigger : ITrigger {
        public List<ITrigger> Sequence { get; set; }
    }

    public class ComboTrigger : ITrigger {
        public List<ITrigger> Sequence { get; set; }
    }

    //public class SystemTrigger : ITrigger {
    //    public string Key { get; set; } // LowPower? ControllerUnlugged?
    //}

    public class SystemAction : IAction {
        public string Key { get; set; } // Reboot, Shutdown, Lock
    }

    public class PadOSTrigger : ITrigger {
        public string Key { get; set; } // ProfileChanged
    }

    public class PadOSAction : IAction {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}