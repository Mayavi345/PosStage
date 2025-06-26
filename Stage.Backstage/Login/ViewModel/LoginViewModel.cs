using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.ReportingServices.Interfaces;
using Stage.BLL.BLL;
using Stage.Presentation.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using Utilities;
using PosStage.MVVM.Models.Implement_Model;
using Utilities.Nlog;
using UIComponent;
using System.Windows;

namespace Stage.Backstage
{
    public class LoginViewModel : ObservableObject, INotifyDataErrorInfo
    {

        private string _userName;
        private string _password;

        public ICommand LoginCommand { get; }
        public ICommand LoginFastCommand { get; set; }
        public ICommand SettingCommand { get; }

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
            LoginCommand = new RelayCommand(OnLogin);
            LoginFastCommand = new RelayCommand(LoginFast);
            SettingCommand = new RelayCommand(OnSetting);
            _errorMessageHelper = new ErrorMessageHelper();
            ErrorsChanged += LoginViewModel_ErrorsChanged;
        }



        private void LoginViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            var a = 0;

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
                    var errorMessage = "密碼錯誤";
                    var propertyName = "Password";
                    RaiseErrorMessage(errorMessage, propertyName);
                    //  ErrorMessage = TextResourceCenter.WarmTextEmoji + "登入失敗";
                    //    _errorMessageHelper.AddError("LogFail", nameof(IsPasswordValid));
                    //MainSystemService.Instance.ObserverUIMessageBox.NotifyObservers(FailText);
                }
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                MainSystemService.Instance.ObserverUIMessageBox.NotifyObservers(e.Message);
            }
        }
        private void LoginSuccess()
        {
            PageManager.Instance.SwitchToPage(EViewPage.EmployeeManagerView);
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
        private void RaiseErrorsChanged(string propertyName)
        {
            _errorMessageHelper.RaiseErrorsChanged(this, propertyName);
        }
        private void OnSetting()
        {
            Stage.Tool.MainWindow toolWindow = new Stage.Tool.MainWindow();
            toolWindow.Show();
        }
        #region INotifyDataErrorInfo
        public IEnumerable GetErrors(string? propertyName)
        {
            return _errorMessageHelper.GetErrors(propertyName);
        }

        internal void Init()
        {
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