using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stage.Backstage.Common;
using Stage.BLL.BLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Utilities;
using PosStage.MVVM.Models;
using PosStage.MVVM.Models.Implement_Model;
using static System.Net.Mime.MediaTypeNames;
using Utilities.Nlog;
using GalaSoft.MvvmLight.Helpers;
using System.ComponentModel.DataAnnotations;
using UIComponent;
using System.Windows.Input;

namespace Stage.Backstage.ViewModel
{
    public class EmployeeManagerViewModel : PageViewModelBase
    {
        #region Field
        BLL.BLL.Service.IEmployeeService _employeeService;
        private readonly EmployeeModel _empty;
        IFilter<IFilterableObject> _filterTextHelper;
        public Action RefreshGridCallBack;
        #endregion
        #region Constructor
        public EmployeeManagerViewModel()
        {
            try
            {
                //初始化系統
                NavBarViewModel = new NavBarViewModel();
                _employeeService = MainSystemService.Instance.EmployeeService;
                _empty = new EmployeeModel();
                RefreshGridCallBack = UpdateDGList;
                AddEmployeeViewModel = new AddEmployeeViewModel(RefreshGridCallBack);
                UpdateEmployeeViewModel = new UpdateEmployeeViewModel(RefreshGridCallBack);
                DataGridSelectionChangedCommand = new RelayCommand<object>(OnDataGridSelectionChanged);

                //初始化顯示資料
                SelectedEmployee = new EmployeeModel();
                //製作員工列表
                EmployeeDGList = new ObservableCollection<EmployeeModel>();
                UpdateDGList();

                _filterTextHelper = new FilterTextHelper(TextToFilter);

                SetDefalutSelectEmployee();
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                MainSystemService.Instance.ShowMessageBox(e.Message);
            }
        }

        private void SetDefalutSelectEmployee()
        {
            SelectedEmployee = EmployeeDGList[0];
            EditCommandAction(SelectedEmployee);
        }
        #endregion
        #region Properties
        private AddEmployeeViewModel _addEmployeeViewModel;
        public AddEmployeeViewModel AddEmployeeViewModel
        {
            get { return _addEmployeeViewModel; }
            set
            {
                if (_addEmployeeViewModel != value)
                {
                    _addEmployeeViewModel = value;
                    RaisePropertyChanged(nameof(AddEmployeeViewModel));
                }
            }
        }
        private UpdateEmployeeViewModel _updateEmployeeViewModel;
        public UpdateEmployeeViewModel UpdateEmployeeViewModel
        {
            get { return _updateEmployeeViewModel; }
            set
            {
                if (_updateEmployeeViewModel != value)
                {
                    _updateEmployeeViewModel = value;
                    RaisePropertyChanged(nameof(UpdateEmployeeViewModel));
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
                    RaisePropertyChanged(nameof(SelectedEmployee));
                }
            }
        }
        private NavBarViewModel _navBarViewModel;
        public NavBarViewModel NavBarViewModel
        {
            get { return _navBarViewModel; }
            set
            {
                if (_navBarViewModel != value)
                {
                    _navBarViewModel = value;
                    RaisePropertyChanged(nameof(NavBarViewModel));
                }
            }
        }
        private ObservableCollection<EmployeeModel> _employeeDGList;
        public ObservableCollection<EmployeeModel> EmployeeDGList
        {
            get { return _employeeDGList; }
            set
            {
                if (_employeeDGList != value)
                {
                    _employeeDGList = value;
                    RaisePropertyChanged(nameof(EmployeeDGList));
                }
            }
        }
        private ICollectionView _employeeCollection;
        public ICollectionView EmployeeCollection
        {
            get { return _employeeCollection; }
            set
            {
                if (_employeeCollection != value)
                {
                    _employeeCollection = value;
                    RaisePropertyChanged(nameof(EmployeeCollection));

                }
            }
        }

        private string _textToFilter;
        public string TextToFilter
        {
            get { return _textToFilter; }
            set
            {
                if (_textToFilter != value)
                {
                    _textToFilter = value;
                    _filterTextHelper.UpdateFilterText(_textToFilter);
                    EmployeeCollection.Filter = _filterTextHelper.ContainsFilter;
                    RaisePropertyChanged(nameof(TextToFilter));
                }
            }
        }
        #endregion
        #region Command
        RelayCommand<EmployeeModel> _deleteCommand;
        RelayCommand<EmployeeModel> _editCommand;
        public RelayCommand<EmployeeModel> EditCommand
        {
            get
            {
                if (_editCommand == null)
                {
                    _editCommand = new RelayCommand<EmployeeModel>(EditCommandAction);
                }
                return _editCommand;
            }
            set { _editCommand = value; }
        }
        public RelayCommand<EmployeeModel> DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand<EmployeeModel>(DeleteCommandAction);
                }
                return _deleteCommand;
            }
            set { _deleteCommand = value; }
        }
        public ICommand DataGridSelectionChangedCommand { get; set; }


        #endregion
        #region Private Method
        private void DeleteCommandAction(EmployeeModel item)
        {
            UIMessageBoxOKCancel.Instance.ShowDialog(() =>
            {
                _employeeService.Remove(item);
                UpdateDGList();
            }, () => { });
    
        }
        private void EditCommandAction(EmployeeModel item)
        {
            //UpdatetCategoriesIndex(item.Role);
            UpdateEmployeeViewModel.FillData(item);
        }
        private void UpdateDGList()
        {
            EmployeeDGList.Clear();
            List<EmployeeModel> itemList = _employeeService.GetItems();
            EmployeeDGList = new ObservableCollection<EmployeeModel>(itemList);
            EmployeeCollection = CollectionViewSource.GetDefaultView(EmployeeDGList);
        }
        private void OnDataGridSelectionChanged(object parameter)
        {
            if (parameter is System.Collections.IList selectedItems)
            {
                //拿到的是SelectedItemCollection 
                var item = selectedItems.Cast<EmployeeModel>().FirstOrDefault();
                if (item == null)
                {
                    //在點選單個item的時候，會造成Null
                }
                else
                {
                    EditCommandAction(item);
                }
            }
            {
                string message = "OnDataGridSelectionChanged  型別錯誤";
                LogManagerSingleton.Instance.PrintLog(message, NLog.LogLevel.Error);
            }
        }
        #endregion

    }

}
