using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadOS.SaveData.ProfileXML;

namespace PadOS.ProfileExecution {
    public class ShowMessageHandler : IActionHandler {
        public bool Enabled { get; set; }

        private string _text;

        public void Init(IAction actionNode) {
            var text = (ShowMessage)actionNode;
            _text = text.Text;
        }

        public void Invoke() {
            System.Windows.MessageBox.Show(_text, "PadOS", System.Windows.MessageBoxButton.OK);
        }

        public void InvokeOff() {
        }
    }
}
