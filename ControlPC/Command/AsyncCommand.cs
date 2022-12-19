using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ControlPC.Command
{
    public class AsyncCommand : ICommand
    {
        private readonly Func<object, Task> execute;
        private bool canExecute = true;

        public event EventHandler CanExecuteChanged;

        public AsyncCommand(Func<object, Task> execute)
        {
            this.execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public async void Execute(object parameter)
        {
            if (canExecute)
            {
                canExecute = false;
                if (CanExecuteChanged != null)
                    CanExecuteChanged(this, EventArgs.Empty);

                try
                {
                    await execute(parameter);
                }
                finally
                {
                    canExecute = true;
                    if (CanExecuteChanged != null)
                        CanExecuteChanged(this, EventArgs.Empty);
                }
            }
        }
    }

    /*public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();
        bool CanExecute();
    }

    public class AsyncCommand : IAsyncCommand
    {
        public event EventHandler CanExecuteChanged;

        private bool _isExecuting;
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private readonly IErrorHandler _errorHandler;

        public AsyncCommand(
        Func<Task> execute,
        Func<bool> canExecute = null,
        IErrorHandler errorHandler = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            _errorHandler = errorHandler;
        }

        public bool CanExecute()
        {
            return !_isExecuting && (_canExecute != null ? true : false);
        }

        public async Task ExecuteAsync()
        {
            if (CanExecute())
            {
                try
                {
                    _isExecuting = true;
                    await _execute();
                }
                finally
                {
                    _isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        #region Explicit implementations
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        void ICommand.Execute(object parameter)
        {
            ExecuteAsync().FireAndForgetSafeAsync(_errorHandler);
        }
        #endregion
    }*/
}