using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PadOS.ProfileSwitcher
{

    public class BackgroundTracker {

        private string _lastProcessName;
        private string _lastWindowTitle;
        private bool _enabled;

        public bool Enabled {
            get { return _enabled; }
            set {
                if (value != _enabled && value == true)
                    new Thread(Poll).Start();
                _enabled = value;
            }
        }

        public event Action<string, string> WindowChanged;

        private StringBuilder _builder = new StringBuilder(128);

        // https://stackoverflow.com/a/48319879
        public static string GetMainModuleFileName(System.Diagnostics.Process process, int buffer = 1024) {
            var fileNameBuilder = new StringBuilder(buffer);
            uint bufferLength = (uint)fileNameBuilder.Capacity + 1;
            return DllImport.Kernel32.QueryFullProcessImageName(process.Handle, 0, fileNameBuilder, ref bufferLength) ?
                fileNameBuilder.ToString() : null;
        }

        private void Poll() {
            while (Enabled)
            {
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
                var hasChanged = false;

                var newProcess = GetMainModuleFileName(firstOrDefault);
                if (newProcess != null && _lastProcessName != newProcess) {
                    hasChanged = true;
                    _lastProcessName = newProcess;
                }

                var length = DllImport.UserInfo32.GetWindowText(hWnd, _builder, _builder.Capacity);
                var windowTitle = _builder.ToString();
                if (length > 0 && _lastWindowTitle != windowTitle) {
                    hasChanged = true;
                    _lastWindowTitle = windowTitle;
                }

                if(hasChanged)
                    WindowChanged?.Invoke(_lastProcessName, _lastWindowTitle);
            }
        }
    }
}
