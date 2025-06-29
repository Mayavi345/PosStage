using GalaSoft.MvvmLight.Command;
using System;

namespace Utilities
{
    public class RelayCommandBase
    {
        public RelayCommandBase(Action<object> action)
        {
            _action = action;
        }
        private Action<object> _action;
        private RelayCommand<object> _Command;
        public RelayCommand<object> Command
        {
            get
            {
                if (_Command == null)
                {
                    _Command = new RelayCommand<object>(_action);
                }
                return _Command;
            }
            set { _Command = value; }
        }
    }
}