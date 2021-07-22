using System;
using System.Windows;

namespace PadOS.Input.BlockNavigator {
    public class Await {
        private Action<FrameworkElement> callback;
        private FrameworkElement a;
        private System.Threading.Tasks.TaskCompletionSource<bool> tcs;

        public Await(FrameworkElement a, Action<FrameworkElement> callback) {
            this.callback = callback;
            this.a = a;
        }

        public Await LayoutUpdated() {
            a.LayoutUpdated += A_LayoutUpdated;
            return this;
        }
        public Await Loaded() {
            a.Loaded += A_Loaded;
            return this;
        }

        public System.Threading.Tasks.Task Task() {
            tcs = new System.Threading.Tasks.TaskCompletionSource<bool>();
            return tcs.Task;
        }

        private void A_LayoutUpdated(object sender, EventArgs e) {
            tcs?.SetResult(true);
            callback?.Invoke(a);
            a.LayoutUpdated -= A_LayoutUpdated;
        }

        private void A_Loaded(object sender, RoutedEventArgs e) {
            tcs?.SetResult(true);
            callback?.Invoke(a);
            a.Loaded -= A_Loaded;
        }
    }
}


