using System.ComponentModel;

namespace Utilities
{
    public class DataErrorInfo : INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _propertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
        private string _errorMessage;

        public bool HasErrors => _propertyNameToErrorsDictionary.Any();

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

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public event PropertyChangedEventHandler PropertyChanged;

 

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddError(string propertyName, string error)
        {
            if (!_propertyNameToErrorsDictionary.ContainsKey(propertyName))
            {
                _propertyNameToErrorsDictionary[propertyName] = new List<string>();
            }
            if (!_propertyNameToErrorsDictionary[propertyName].Contains(error))
            {
                _propertyNameToErrorsDictionary[propertyName].Add(error);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        public void ClearErrors(string propertyName)
        {
            if (_propertyNameToErrorsDictionary.ContainsKey(propertyName))
            {
                _propertyNameToErrorsDictionary.Remove(propertyName);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        System.Collections.IEnumerable INotifyDataErrorInfo.GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return _propertyNameToErrorsDictionary.Values.SelectMany(errors => errors);
            }
            if (_propertyNameToErrorsDictionary.ContainsKey(propertyName))
            {
                return _propertyNameToErrorsDictionary[propertyName];
            }
            return null;
        }
    }
}
