#define Test
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.EntityFrameworkCore;
using Stage.BLL.BLL;
using Stage.Presentation.Common;
using Stage.Presentation.MVVM.MainPage.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Utilities;
using PosStage.MVVM.Models;
using PosStage.MVVM.Models.Implement_Model;
using PosStage.MVVM.View;
using static System.Net.Mime.MediaTypeNames;
using Utilities.Nlog;
using UIComponent;

namespace PosStage.MVVM.ViewModel
{
    public class LoginViewModel : ObservableObject, INotifyDataErrorInfo
    {

        private string _userName;
        private string _password;

        public ICommand LoginCommand { get; }
        public ICommand LoginFastCommand { get; set; }
        public ICommand SettingCommand { get; }

        private LoginViewModel _loginViewModel;
        private ErrorMessageHelper _errorMessageHelper;

        public bool IsDebugModel = false;
        public Visibility DebugVisibility => IsDebugModel ? Visibility.Visible : Visibility.Hidden;

        public LoginViewModel()
        {
#if Developer
            IsDebugModel = true;
#else
            IsDebugModel = false;
#endif
            _loginViewModel = this;
            LoginCommand = new RelayCommand(OnLogin);
            LoginFastCommand = new RelayCommand(LoginFast);
            SettingCommand = new RelayCommand(OnSetting);
            _errorMessageHelper = new ErrorMessageHelper();
            ErrorsChanged += LoginViewModel_ErrorsChanged;

        }

        private void LoginViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            var test = 0;

        }

        #region Propertis
        [Required(ErrorMessage = "請輸入名字")]
        public string Username
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        #endregion
        private void LoginFast()
        {
            string userNmame = Username = "Admin";
            string password = Password = "123";
            if (IsPasswordValid(userNmame, password))
            {
                LoginSuccess();
            }
            else
            {

                MainSystemService.Instance.ObserverUIMessageBox.NotifyObservers(TextResourceCenter.FailText);
            }
        }
        private void OnLogin()
        {
            try
            {
                string userNmame = Username;
                string password = Password;
                if (IsPasswordValid(userNmame, password))
                {
                    LoginSuccess();
                }
                else
                {
                    //    _errorMessageHelper.ErrorMessage = TextResourceCenter.WarmTextEmoji + "登入失敗";
                    var errorMessage = "密碼錯誤";
                    var propertyName = "Password";
                    RaiseErrorMessage(errorMessage, propertyName);
                    //MainSystemService.Instance.ObserverUIMessageBox.NotifyObservers(FailText);
                }
            }
            catch (Exception e)
            {
                MainSystemService.Instance.ObserverUIMessageBox.NotifyObservers(e.Message);
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
            }
        }
        private void LoginSuccess()
        {
            PageManager.Instance.SwitchToPage(EViewPage.MainPageView);
        }

        private bool IsPasswordValid(string account, string password)
        {
            _errorMessageHelper.ErrorMessage = string.Empty;
            _errorMessageHelper.ClearErrors(nameof(Username));
            _errorMessageHelper.ClearErrors(nameof(Password));
            _errorMessageHelper.ValidateProperty(Username, "Username", this);
            _errorMessageHelper.ValidateProperty(Password, "Password", this);

            EmployeeModel user = MainSystemService.Instance.EmployeeService.GetItemByAccount(account);
            if (user != null)
            {
                if (user.Password == password)
                {
                    MainSystemService.Instance.EmployeeService.SetCurrentEmployee(user);
                    MainDataCenter.Instance.RefreshNavBar.NotifyObservers(() => { });

                    return true;
                }
                return false;
            }
            else
            {
                var errorMessage = "找不到該使用者";
                var propertyName = "Username";
                RaiseErrorMessage(errorMessage, propertyName);
                return false;
            }
        }

        private void RaiseErrorMessage(string errorMessage, string propertyName)
        {
            _errorMessageHelper.AddError(errorMessage, propertyName);
            RaiseErrorsChanged(propertyName);
        }

        private void OnSetting()
        {
            Stage.Tool.MainWindow toolWindow = new Stage.Tool.MainWindow();
            toolWindow.Show();
        }
        private void ValidateProperty(string propertyName)
        {
            // 清除之前的錯誤訊息
            _errorMessageHelper.propertyNameToErrorsDictionary.Remove(propertyName);
            RaiseErrorsChanged(propertyName);

            switch (propertyName)
            {
                case "Username":
                    // 基於 DataAnnotations 屬性驗證
                    var usernameValidationContext = new ValidationContext(this)
                    {
                        MemberName = propertyName
                    };
                    var usernameValidationResults = new List<ValidationResult>();
                    if (!Validator.TryValidateProperty(Username, usernameValidationContext, usernameValidationResults))
                    {
                        _errorMessageHelper.propertyNameToErrorsDictionary[propertyName] = usernameValidationResults.Select(result => result.ErrorMessage).ToList();
                        RaiseErrorsChanged(propertyName);
                        foreach (var validationResult in usernameValidationResults)
                        {
                            _errorMessageHelper.ErrorMessage = validationResult.ErrorMessage;
                        }
                    }
                    break;

                case "Password":
                    #region 自訂驗證規則，以註解
                    //// 自訂驗證規則
                    //if (string.IsNullOrWhiteSpace(Password))
                    //{
                    //    _propertyNameToErrorsDictionary[propertyName] = new List<string> { "Password is required." };
                    //    RaiseErrorsChanged(propertyName);
                    //}
                    #endregion
                    // 基於 DataAnnotations 屬性驗證
                    var passwordValidationContext = new ValidationContext(this)
                    {
                        MemberName = propertyName
                    };
                    var passwordValidationResults = new List<ValidationResult>();
                    if (!Validator.TryValidateProperty(Username, passwordValidationContext, passwordValidationResults))
                    {
                        _errorMessageHelper.propertyNameToErrorsDictionary[propertyName] = passwordValidationResults.Select(result => result.ErrorMessage).ToList();
                        RaiseErrorsChanged(propertyName);
                        foreach (var validationResult in passwordValidationResults)
                        {
                            _errorMessageHelper.ErrorMessage = validationResult.ErrorMessage;
                        }
                    }
                    break;
                default:
                    break;
            }
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

        private void RaiseErrorsChanged(string propertyName)
        {
            _errorMessageHelper.RaiseErrorsChanged(this, propertyName);
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            return _errorMessageHelper.GetErrors(propertyName);
        }

        internal void Init()
        {
        }

        public bool HasErrors => _errorMessageHelper.HasErrors;

    }
}