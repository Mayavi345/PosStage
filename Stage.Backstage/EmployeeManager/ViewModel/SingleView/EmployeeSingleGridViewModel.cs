using PosStage.MVVM.Models;
using PosStage.MVVM.Models.Implement_Model;
using Stage.BLL.BLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Media;
using UIComponent;
using Utilities;
using Utilities.Nlog;

namespace Stage.Backstage.ViewModel
{
    public class EmployeeSingleGridViewModel : SingleViewFormViewModelBase
    {
        #region Field
        BLL.BLL.Service.IEmployeeService _employeeService;
        public override List<Action> ValidateActions { get; set; }

        #endregion
        #region Constructor
        public EmployeeSingleGridViewModel()
        {
            try
            {
                _employeeService = MainSystemService.Instance.EmployeeService;
                SelectedEmployee = new EmployeeModel();
                GenderList = EmployeeModel.GetGenderList();
                //更新權限腳色
                RoleList = new ObservableCollection<IComboBoxGenericItem<RoleViewModel>>(GetRoles());
                //設定defalut item
                GenderSelectedItem = new GenderModel();
                RoleListSelectedItem = new RoleViewModel();
                GenderSelectedItem = GenderList[0];
                RoleListSelectedItem = RoleList[0];

            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                MainSystemService.Instance.ShowMessageBox(e.Message);
            }
        }


        #endregion
        #region Properties
        private ObservableCollection<IComboBoxGenericItem<RoleViewModel>> items;
        public ObservableCollection<IComboBoxGenericItem<RoleViewModel>> RoleList
        {
            get { return items; }
            set
            {
                items = value;
                RaisePropertyChanged();
            }
        }
        private IComboBoxGenericItem<RoleViewModel> selectedItem;
        public IComboBoxGenericItem<RoleViewModel> RoleListSelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                SelectedEmployee.Role = RoleListSelectedItem.Value;
                RaisePropertyChanged(nameof(RoleListSelectedItem));
            }
        }
        private GenderModel _genderSelectedItem;
        public GenderModel GenderSelectedItem
        {
            get { return _genderSelectedItem; }
            set
            {
                if (_genderSelectedItem != value)
                {
                    _genderSelectedItem = value;
                    SelectedEmployee.Gender = GenderSelectedItem;
                    RaisePropertyChanged(nameof(GenderSelectedItem));
                }
            }
        }
        private List<GenderModel> _genderList;
        public List<GenderModel> GenderList
        {
            get { return _genderList; }
            set
            {
                if (_genderList != value)
                {
                    _genderList = value;
                    RaisePropertyChanged(nameof(GenderList));
                }
            }
        }
        //TODO 目前使用INotifyDataErrorInfo的通知打不到底下一層的model，先用這方式解決
        [Required(ErrorMessage = TextResourceCenter.WarmTextEmoji + "請輸入名字")]
        [MaxLength(12, ErrorMessage = TextResourceCenter.WarmTextEmoji + "長度不能超過12")]
        public string Name
        {
            get { return SelectedEmployee.Name; }
            set
            {
                if (SelectedEmployee.Name != value)
                {
                    SelectedEmployee.Name = value;
                    _errorMessageHelper.ClearErrors(this, "Name");

                    RaisePropertyChanged(nameof(Name));
                }
            }
        }
        [Required(ErrorMessage = TextResourceCenter.WarmTextEmoji + "請輸入帳號")]
        [MaxLength(10, ErrorMessage = TextResourceCenter.WarmTextEmoji + "長度不能超過10")]
        public string Account
        {
            get { return SelectedEmployee.Account; }
            set
            {
                if (SelectedEmployee.Account != value)
                {
                    SelectedEmployee.Account = value;
                    _errorMessageHelper.ClearErrors(this, "Account");

                    RaisePropertyChanged(nameof(Account));
                }
            }
        }
        [Required(ErrorMessage = TextResourceCenter.WarmTextEmoji + "請輸入密碼")]
        [MaxLength(10, ErrorMessage = TextResourceCenter.WarmTextEmoji + "長度不能超過10")]
        public string Password
        {
            get { return SelectedEmployee.Password; }
            set
            {
                if (SelectedEmployee.Password != value)
                {
                    SelectedEmployee.Password = value;
                    _errorMessageHelper.ClearErrors(this, "Password");

                    RaisePropertyChanged(nameof(Password));
                }
            }
        }
        private EmployeeModel _selectedEmployee;
        public EmployeeModel SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                if (_selectedEmployee != value)
                {
                    _selectedEmployee = value;
                    RaisePropertyChanged(nameof(Name));
                    RaisePropertyChanged(nameof(Account));
                    RaisePropertyChanged(nameof(Password));
                    RaisePropertyChanged(nameof(GenderSelectedItem));
                    RaisePropertyChanged(nameof(RoleListSelectedItem));

                    RaisePropertyChanged(nameof(SelectedEmployee));
                }
            }
        }

        #endregion
        #region Public method
        public override void InitValidate()
        {
            ValidateActions = new List<Action>();
            ValidateActions.Add(() =>
            {
                var value = Name;
                var propertyName = "Name";
                var validationContext = this;
                _errorMessageHelper.ValidateProperty(value, propertyName, validationContext);
            });
            ValidateActions.Add(() =>
            {
                _errorMessageHelper.ValidateProperty(Account, "Account", this);
            });
            ValidateActions.Add(() =>
            {
                _errorMessageHelper.ValidateProperty(Password, "Password", this);
            });
        }
        public override void ClearData()
        {
            //TODO new優化
            SelectedEmployee = new EmployeeModel();
            GenderSelectedItem = GenderList[0];
            RoleListSelectedItem = RoleList[0];

            _errorMessageHelper.ClearErrors(this,"Name");
        }

        public override void InitSingleViewData<T>(T data)
        {
            try
            {
                var selectEmployeeModel = data as EmployeeModel;
                SelectedEmployee = new EmployeeModel(selectEmployeeModel);

                //TODO Combo 因為物件要拿實際的物件，因此先用此方法實作。之後想如何處理
                var CurrentRoleSelectedItem = RoleList.Where(x => x.DisplayValue == selectEmployeeModel.Role.DisplayValue).FirstOrDefault();
                RoleListSelectedItem = CurrentRoleSelectedItem;

                var currentGenderModel = GenderList.Where(x => x.Id == selectEmployeeModel.Gender.Id).FirstOrDefault();
                GenderSelectedItem = currentGenderModel;
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                MainSystemService.Instance.ShowMessageBox(e.Message);
            }
        }
        #endregion
        #region Private method
        private List<RoleViewModel> GetRoles()
        {
            return _employeeService.GetRoles();
        }
        #endregion
    }
  
}
