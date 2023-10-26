using System;
using System.Windows.Input;

namespace Alarms.UI
{
    public class CommonCommand : ICommand
    {
        private readonly Action _action;
        private readonly Func<bool>? _canExecute;

        public event EventHandler? CanExecuteChanged;
        public CommonCommand(Action action, Func<bool>? canExecute = null)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null)
                return true;

            return _canExecute();
        }

        public void Execute(object? parameter)
        {
            _action();
        }
    }
}
