using System;
using System.Threading.Tasks;
using System.Windows;

namespace PadOS.CustomControls
{
    public partial class ConfirmDialogue : Window {
        public ConfirmDialogue() {
            InitializeComponent();
            _taskCompletionSource = new TaskCompletionSource<DialogueResult>();
        }

        public static async Task<DialogueResult> ShowDialogAsync() {
            var dialog = new ConfirmDialogue();
            dialog.Show();
            return await dialog.Task();
        }

        public event Action<DialogueResult> UserClicked;

        public async Task<DialogueResult> Task() {
            return await _taskCompletionSource.Task;
        }

        public enum DialogueResult {
            Cancel,
            Yes,
            No
        }

        private readonly TaskCompletionSource<DialogueResult> _taskCompletionSource;

        private void SetResult(DialogueResult res) {
            _taskCompletionSource.SetResult(res);
            UserClicked?.Invoke(res);
            Close();
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e) =>
            SetResult(DialogueResult.Yes);

        private void ButtonNo_Click(object sender, RoutedEventArgs e) =>
            SetResult(DialogueResult.No);

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) =>
            SetResult(DialogueResult.Cancel);

        private void WindowCancel_Click(object sender, EventArgs args) =>
            SetResult(DialogueResult.Cancel);
    }
}
