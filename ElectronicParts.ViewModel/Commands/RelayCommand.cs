// ***********************************************************************
// Assembly         : ElectronicParts.ViewModel
// Author           : 
// ***********************************************************************
// <copyright file="RelayCommand.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the RelayCommand class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.ViewModels.Commands
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Represents the RelayCommand class of the ElectronicParts program.
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// The action that is executed when the command is executed.
        /// </summary>
        private readonly Action<object> action;

        /// <summary>
        /// A predicate which indicates whether the command can execute or not.
        /// </summary>
        private readonly Predicate<object> canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="action">The action that is executed when the command is executed.</param>
        /// <param name="canExecute">A predicate which indicates whether the command can execute or not.</param>
        public RelayCommand(Action<object> action, Predicate<object> canExecute = null)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.canExecute = canExecute;
        }

        /// <summary>
        /// A event that is fired when the <see cref="CanExecute(object)"/> value changes.
        /// </summary>
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

        /// <summary>
        /// Indicates whether the command can execute or not.
        /// </summary>
        /// <param name="parameter">A value used to decide if the command can execute or not.</param>
        /// <returns>A value indicating whether the command can execute or not.</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute?.Invoke(parameter) ?? true;
        }

        /// <summary>
        /// Executes the previously defined action.
        /// </summary>
        /// <param name="parameter">The parameter used for the execution of the previously defined action.</param>
        public void Execute(object parameter)
        {
            this.action(parameter);
        }
    }
}
