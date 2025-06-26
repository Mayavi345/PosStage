using GalaSoft.MvvmLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UIComponent;

namespace UIComponent
{
    public abstract class SingleViewFormViewModelBase : ObservableObject, INotifyDataErrorInfo
    {
        protected ErrorMessageHelper _errorMessageHelper;
        public abstract List<Action> ValidateActions { get; set; }
        public SingleViewFormViewModelBase()
        {
            _errorMessageHelper = new ErrorMessageHelper();
            InitValidate();
        }

        public abstract void InitValidate();
        public abstract void ClearData();
        public abstract void InitSingleViewData<T>(T data);
        public bool CheckIsValidate()
        {
            _errorMessageHelper.ErrorMessage = string.Empty;
            //_errorMessageHelper.ClearErrors(nameof(Name));
            if (ValidateActions == null)
                return false;
            foreach (var action in ValidateActions)
            {
                action?.Invoke();
            }
            if (_errorMessageHelper.ErrorMessage.Length == 0)
                return true;
            else
                return false;
        }
        #region INotifyDataErrorInfo
        public IEnumerable GetErrors(string? propertyName)
        {
            return _errorMessageHelper.GetErrors(propertyName);
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
        {
            add
            {
                _errorMessageHelper.ErrorsChanged += value;
            }
            remove
            {
                _errorMessageHelper.ErrorsChanged -= value;
            }
        }
        public bool HasErrors => _errorMessageHelper.HasErrors;
        #endregion
    }

}
