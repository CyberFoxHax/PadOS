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

        // https://stackoverflow.com/a/48319879
        public static string GetMainModuleFileName(System.Diagnostics.Process process, int buffer = 1024) {
            var fileNameBuilder = new StringBuilder(buffer);
            uint bufferLength = (uint)fileNameBuilder.Capacity + 1;
            return DllImport.Kernel32.QueryFullProcessImageName(process.Handle, 0, fileNameBuilder, ref bufferLength) ?
                fileNameBuilder.ToString() : null;
        }

        private void Poll() {
            while (Enabled) {
                Thread.Sleep(100);
                var hWnd = DllImport.UserInfo32.GetForegroundWindow();

                int processId;
                DllImport.UserInfo32.GetWindowThreadProcessId(hWnd, out processId);
                if (processId == 0)
                    continue;

                var current = System.Diagnostics.Process.GetCurrentProcess();

                var firstOrDefault = System.Diagnostics.Process.GetProcesses().FirstOrDefault(p => p.Id == processId);
                if (firstOrDefault == null)
                    continue;
                var newProcess = GetMainModuleFileName(firstOrDefault);
                if (newProcess == null || _lastProcessName == newProcess)
                    continue;

                if(Enabled)
                    ProcessChanged?.Invoke(_lastProcessName, newProcess);
                _lastProcessName = newProcess;
            }
        }
    }
}
