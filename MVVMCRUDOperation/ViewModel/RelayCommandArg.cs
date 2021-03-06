﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMCRUDOperation.ViewModel
{
    public class RelayCommandArg : ICommand
    {
        // Event that fires when the enabled/disabled state of the cmd changes
        public event EventHandler CanExecuteChanged;
        // Delegate for method to call when the cmd needs to be executed
        private readonly Action<object> _targetExecuteMethod;
        // Delegate for method that determines if cmd is enabled/disabled
        private readonly Predicate<object> _targetCanExecuteMethod;
        public bool CanExecute(object parameter)
        {
            return _targetCanExecuteMethod == null || _targetCanExecuteMethod(parameter);
        }
        public void Execute(object parameter)
        {
            // Call the delegate if it's not null
            _targetExecuteMethod?.Invoke(parameter);
        }
        public RelayCommandArg(Action<object> executeMethod, Predicate<object> canExecuteMethod = null)
        {
            _targetExecuteMethod = executeMethod;
            _targetCanExecuteMethod = canExecuteMethod;
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
