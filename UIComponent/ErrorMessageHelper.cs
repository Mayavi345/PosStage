using GalaSoft.MvvmLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Utilities;

namespace UIComponent
{
    public class ErrorMessageHelper : ObservableObject, INotifyDataErrorInfo
    {
        /// <summary>
        /// 已經被觸發的Error資料
        /// </summary>
        private Dictionary<string, object> _propertyOnErrorsDic;
        public ErrorMessageHelper()
        {
            propertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
            _propertyOnErrorsDic = new Dictionary<string, object>();
        }
        public void InitErrorMessage()
        {
            ErrorMessage = string.Empty;
        }
        public bool HasErrors => propertyNameToErrorsDictionary.Any();
        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    RaisePropertyChanged(nameof(HasErrorMessage));
                    RaisePropertyChanged(nameof(ErrorMessage));
                }
            }
        }
        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);


        public Dictionary<string, List<string>> propertyNameToErrorsDictionary;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public void ClearErrors(string propertyName)
        {
            propertyNameToErrorsDictionary.Remove(propertyName);
        }
        public void AddError(string errorMessage, string propertyName)
        {
            if (propertyNameToErrorsDictionary.ContainsKey(propertyName) == false)
            {
                propertyNameToErrorsDictionary.Add(propertyName, new List<string>());
            }

            propertyNameToErrorsDictionary[propertyName].Add(errorMessage);
        }
        //private void OnErrorsChange(string propertyName)
        //{
        //    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        //}

        public IEnumerable GetErrors(string? propertyName)
        {
            return propertyNameToErrorsDictionary.GetValueOrDefault(propertyName, new List<string>());
        }
        public void ValidateProperty(object value, string propertyName, object validationContext)
        {
            // 清除之前的錯誤訊息
            ClearErrors(nameof(propertyName));
            RaiseErrorsChanged(validationContext, propertyName);
            // 基於 DataAnnotations 屬性驗證
            var usernameValidationContext = new ValidationContext(validationContext)
            {
                MemberName = propertyName
            };
            var usernameValidationResults = new List<ValidationResult>();
            if (!Validator.TryValidateProperty(value, usernameValidationContext, usernameValidationResults))
            {
                propertyNameToErrorsDictionary[propertyName] = usernameValidationResults.Select(result => result.ErrorMessage).ToList();
                foreach (var validationResult in usernameValidationResults)
                {
                    ErrorMessage = validationResult.ErrorMessage;
                    AddError(ErrorMessage, propertyName);
                }
                RaiseErrorsChanged(validationContext, propertyName);
            }
        }
        public void RaiseErrorsChanged(object instance, string propertyName)
        {
            ErrorsChanged?.Invoke(instance, new DataErrorsChangedEventArgs(propertyName));
            if (!_propertyOnErrorsDic.ContainsKey(propertyName))
                _propertyOnErrorsDic.Add(propertyName, instance);
        }

        public void ClearErrors(object instance, string propertyName)
        {
            propertyNameToErrorsDictionary.Remove(propertyName);
            RaiseErrorsChanged(instance, propertyName);
        }
        public void ClearAllError()
        {
            foreach (var item in _propertyOnErrorsDic)
            {
                RaiseErrorsChanged(item.Value, item.Key);
            }
            propertyNameToErrorsDictionary.Clear();
        }
    }
}
