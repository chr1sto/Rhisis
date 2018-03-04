using System;
using System.Windows.Input;

namespace Rhisis.Tools.Core.MVVM
{
    public sealed class Command : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;
        
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Creates a new <see cref="Command"/>.
        /// </summary>
        /// <param name="action">Action to execute</param>
        public Command(Action action)
            : this(o => action())
        {
        }

        /// <summary>
        /// Creates a new <see cref="Command"/>.
        /// </summary>
        /// <param name="execute"></param>
        public Command(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="Command"/>.
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public Command(Action<object> execute, Predicate<object> canExecute)
        {
            this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this._canExecute = canExecute;
        }

        /// <summary>
        /// Determines if the command can be executed.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter) => this._canExecute?.Invoke(parameter) ?? true;

        /// <summary>
        /// Execute the command action.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter) => this._execute(parameter);
    }
}
