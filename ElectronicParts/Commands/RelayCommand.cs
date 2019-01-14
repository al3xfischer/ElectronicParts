using System;
using System.Windows.Input;

namespace ElectronicParts.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> action;

        private readonly Predicate<object> canExecute;

        public RelayCommand(Action<object> action, Predicate<object> canExecute = null)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            this.action(parameter);
        }
    }
}
