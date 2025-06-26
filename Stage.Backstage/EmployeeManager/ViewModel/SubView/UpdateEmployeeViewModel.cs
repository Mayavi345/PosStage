using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PosStage.MVVM.Models.Implement_Model;
using Stage.BLL.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Utilities;
using Utilities.Nlog;

namespace Stage.Backstage.ViewModel
{
    public class UpdateEmployeeViewModel : ObservableObject
    {
        #region Field
        private readonly EmployeeSingleGridViewModel _empty;
        BLL.BLL.Service.IEmployeeService _employeeService;
        private Action _refreshGridCallBack;
        #endregion
        #region Constructor
        public UpdateEmployeeViewModel(Action refreshGridCallBack)
        {
            try
            {
                IsUpdateEditEnable = false;

                _employeeService = MainSystemService.Instance.EmployeeService;
                CurrentEmployeeSingleGridViewModel = new EmployeeSingleGridViewModel();
                _empty = new EmployeeSingleGridViewModel();
                _refreshGridCallBack = refreshGridCallBack;
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                MainSystemService.Instance.ShowMessageBox(e.Message);
            }
        }

        #endregion
        #region Properties
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
        private bool _isUpdateEditEnable;
        public bool IsUpdateEditEnable
        {
            get { return _isUpdateEditEnable; }
            set
            {
                if (_isUpdateEditEnable != value)
                {
                    _isUpdateEditEnable = value;
                    RaisePropertyChanged(nameof(IsUpdateEditEnable));
                }
            }
        }
        #endregion
        #region Command
        RelayCommand<EmployeeModel> _updateCommand;
        public RelayCommand<EmployeeModel> UpdateCommand
        {
            get
            {
                if (_updateCommand == null)
                {
                    _updateCommand = new RelayCommand<EmployeeModel>(UpdateCommandAction);
                }
                return _updateCommand;
            }
            set { _updateCommand = value; }
        }
        RelayCommand _updateCancelCommand;
        public RelayCommand UpdateCancelCommand
        {
            get
            {
                if (_updateCancelCommand == null)
                {
                    _updateCancelCommand = new RelayCommand(UpdateCancelCommandAction);
                }
                return _updateCancelCommand;
            }
            set { _updateCancelCommand = value; }
        }
        ICommand _editCommand;
        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                {
                    _editCommand = new RelayCommand(EditCommandAction);
                }
                return _editCommand;
            }
            set { _editCommand = value; }
        }
        #endregion
        #region Public Method
        public void FillData(EmployeeModel item)
        {
            CurrentEmployeeSingleGridViewModel.InitSingleViewData(item);
        }
        #endregion
        #region Private Method
        private void EditCommandAction()
        {
            IsUpdateEditEnable = true;
        }
        private void UpdateCommandAction(EmployeeModel item)
        {
            try
            {
                if (!CurrentEmployeeSingleGridViewModel.CheckIsValidate())
                {
                    MainSystemService.Instance.ObserverUIMessageBox.NotifyObservers(TextResourceCenter.FiledValidateFail);
                    return;
                }
                if (!_employeeService.CheckExistAccount(item, true))
                {
                    IsUpdateEditEnable = false;
                    _employeeService.Update(item);
                    _refreshGridCallBack?.Invoke();
                    MainSystemService.Instance.ObserverUIMessageBox.NotifyObservers(TextResourceCenter.AddSuccess);
                    CurrentEmployeeSingleGridViewModel.ClearData();
                }
                else
                {
                    MainSystemService.Instance.ShowMessageBox(TextResourceCenter.SystemHaveThisMember);
                }

            }
            catch (Exception e)
            {
                CurrentEmployeeSingleGridViewModel.ClearData();
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                MainSystemService.Instance.ShowMessageBox(e.Message);
            }
        }
        private void UpdateCancelCommandAction()
        {
            IsUpdateEditEnable = false;

            CurrentEmployeeSingleGridViewModel.ClearData();

            _refreshGridCallBack?.Invoke();

        }
        #endregion
    }
}
