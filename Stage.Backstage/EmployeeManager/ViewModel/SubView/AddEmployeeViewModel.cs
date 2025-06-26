using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Helpers;
using PosStage.MVVM.Models.Implement_Model;
using Stage.BLL.BLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Nlog;
using Utilities;

namespace Stage.Backstage.ViewModel
{
    public class AddEmployeeViewModel : ObservableObject
    {
        #region Field
        BLL.BLL.Service.IEmployeeService _employeeService;
        private Action _refreshGridCallBack;
        #endregion
        #region Constructor
        public AddEmployeeViewModel(Action refreshGridCallBack)
        {
            try
            {
                _employeeService = MainSystemService.Instance.EmployeeService;
                CurrentEmployeeSingleGridViewModel = new EmployeeSingleGridViewModel();
                _refreshGridCallBack = refreshGridCallBack;
                IsPassValidate = false;
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                MainSystemService.Instance.ShowMessageBox(e.Message);
            }
        }
        #endregion
        #region Properties
        public bool IsPassValidate { get; set; }

        private EmployeeSingleGridViewModel _currentEmployeeSingleGridViewModel;
        public EmployeeSingleGridViewModel CurrentEmployeeSingleGridViewModel
        {
            get { return _currentEmployeeSingleGridViewModel; }
            set
            {
                if (_currentEmployeeSingleGridViewModel != value)
                {
                    _currentEmployeeSingleGridViewModel = value;
                    RaisePropertyChanged(nameof(CurrentEmployeeSingleGridViewModel));
                }
            }
        }
        #endregion
        #region Command
        RelayCommand<EmployeeModel> _addCommand;
        public RelayCommand<EmployeeModel> AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new RelayCommand<EmployeeModel>(AddCommandAction);
                }
                return _addCommand;
            }
            set { _addCommand = value; }
        }

        #endregion
        #region Private Method
        private void AddCommandAction(EmployeeModel item)
        {
            try
            {
                if (CurrentEmployeeSingleGridViewModel.CheckIsValidate() || IsPassValidate)
                {
                    var newItem = new EmployeeModel()
                    {
                        Name = item.Name,
                        Account = item.Account,
                        Password = item.Password,
                        Gender = item.Gender,
                        Role = item.Role,
                    };
                    if (!_employeeService.CheckExistAccount(newItem))
                    {
                        _employeeService.Add(newItem);
                        _refreshGridCallBack?.Invoke();
                        MainSystemService.Instance.ShowMessageBox(TextResourceCenter.AddSuccess);
                    }
                    else
                    {
                        MainSystemService.Instance.ShowMessageBox(TextResourceCenter.SystemHaveThisMember);
                    }

                    CurrentEmployeeSingleGridViewModel.ClearData();
                }
                else
                {
                    MainSystemService.Instance.ShowMessageBox(TextResourceCenter.FiledValidateFail);
                }


            }
            catch (Exception e)
            {
                CurrentEmployeeSingleGridViewModel.ClearData();
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                MainSystemService.Instance.ShowMessageBox(e.Message);
            }
        }
        #endregion
    }
}
