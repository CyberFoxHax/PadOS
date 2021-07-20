using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PadOS.ProfileSwitcher
{

    public class BackgroundTracker {

        private string _lastProcessName;

        private bool _enabled;

        public bool Enabled {
            get { return _enabled; }
            set {
                if (value != _enabled && value == true)
                    new Thread(Poll).Start();
                _enabled = value;
            }
        }

        public delegate void ProcessChangedEvent(string oldProcess, string newProcess);
        public event ProcessChangedEvent ProcessChanged;

        private void Poll() {
            while (Enabled) {
                Thread.Sleep(100);
                var hWnd = DllImport.UserInfo32.GetForegroundWindow();

                int processId;
                DllImport.UserInfo32.GetWindowThreadProcessId(hWnd, out processId);

                var current = System.Diagnostics.Process.GetCurrentProcess();

                var firstOrDefault = System.Diagnostics.Process.GetProcesses().FirstOrDefault(p => p.Id == processId);
                if (firstOrDefault == null || _lastProcessName == firstOrDefault.ProcessName)
                    continue;
                var newProcess = firstOrDefault.ProcessName;
                if (newProcess == null)
                    continue;

                ProcessChanged?.Invoke(_lastProcessName, newProcess);
                _lastProcessName = newProcess;
            }
        }
    }
}
