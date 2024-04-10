using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.Commands
{
    public class RelayParameterCommand :ICommand
    {
       
            private readonly Action<object> _action;
            private readonly Func<object, bool> _canExecute;

            public RelayParameterCommand(Action<object> action, Func<object, bool> canExecute = null)
            {
                _action = action ?? throw new ArgumentNullException(nameof(action));
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute == null || _canExecute(parameter);
            }

            public void Execute(object parameter)
            {
                _action(parameter);
            }

            public event EventHandler CanExecuteChanged;
    }

    
}
