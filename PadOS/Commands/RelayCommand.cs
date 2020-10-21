using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Threading;

namespace PadOS.Commands{
	public sealed class RelayCommand : ICommand {
		public RelayCommand(Action execute, Func<bool> canExecute = null) {
			if (execute == null)
				throw new ArgumentNullException("execute");
			_execute = execute;
			_canExecute = canExecute;
		}

        public RelayCommand(ICommand editInputSimulatorCommand) {
            _editInputSimulatorCommand = editInputSimulatorCommand;
        }

        private readonly Func<bool> _canExecute;
		private readonly Action _execute;
        private ICommand _editInputSimulatorCommand;

        public event EventHandler CanExecuteChanged {
			add		{ CommandManager.RequerySuggested += value; }
			remove	{ CommandManager.RequerySuggested -= value; }
		}

		[DebuggerStepThrough]
		public bool CanExecute(object parameter) {
			return _canExecute == null || _canExecute();
		}

		public void Execute(object parameter){
			App.GlobalDispatcher.BeginInvoke(_execute);
		}
	}

	public sealed class RelayCommand<T> : ICommand {
		public RelayCommand(RelayDelegate execute, CanExecuteDelegate canExecute=null) {
			if (execute == null) {
				throw new ArgumentNullException("execute");
			}
			_execute = execute;
			_canExecute = canExecute;
		}

		public delegate void RelayDelegate(T p);
		public delegate bool CanExecuteDelegate(T p);

		private readonly CanExecuteDelegate _canExecute;
		private readonly RelayDelegate _execute;

		public event EventHandler CanExecuteChanged {
			add		{ CommandManager.RequerySuggested += value; }
			remove	{ CommandManager.RequerySuggested -= value; }
		}

		[DebuggerStepThrough]
		public bool CanExecute(object parameter) {
			return _canExecute == null || _canExecute((T)parameter);
		}

		public void Execute(object parameter) {
			App.GlobalDispatcher.BeginInvoke(new Action(()=>_execute((T) parameter)));
		}
	}
}