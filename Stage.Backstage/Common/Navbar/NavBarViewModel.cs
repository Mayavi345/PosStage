using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stage.Presentation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Utilities;
using PosStage.MVVM.Models;
using Stage.BLL.BLL;
using State.CommonWPF;

namespace Stage.Backstage.Common
{
    public class NavBarViewModel : ObservableObject
    {
        public NavBarViewModel()
        {
            SwitchEmployeeCommand = new RelayCommandBase(SwitchEmployeeCommandAction).Command;
            SwitchProductCommand = new RelayCommandBase(SwitchProductCommandAction).Command;
            SwitchReportCommand = new RelayCommandBase(SwitchReportCommandAction).Command;
            LoginOutCommand = new RelayCommand(LoginOutAction);

            MainDataCenter.Instance.RefreshNavBar.RegisterObserver(new RefreshEmployeeObserver(UpdateMember));

        }

        public void UpdateMember()
        {
            EmployeeName = MainSystemService.Instance.EmployeeService.CurrentEmployee.Name;
            EmployeeGender = MainSystemService.Instance.EmployeeService.CurrentEmployee.Gender.Name;
        }

        private string _name;
        public string EmployeeName
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged(nameof(EmployeeName));
            }
        }
        private string _gender;
        public string EmployeeGender
        {
            get => _gender;
            set
            {
                _gender = value;
                RaisePropertyChanged(nameof(EmployeeGender));
            }
        }


        public ICommand SwitchEmployeeCommand { get; set; }
        public ICommand SwitchProductCommand { get; set; }
        public ICommand SwitchReportCommand { get; set; }
        public ICommand LoginOutCommand { get; set; }


        private void SwitchReportCommandAction(object pare)
        {
            CommonViewSystem.Instance.ReportMainWindow.Show();
        }

        private void SwitchProductCommandAction(object pare)
        {
            PageManager.Instance.SwitchToPage(EViewPage.ProductManagerView);
        }

        private void SwitchEmployeeCommandAction(object pare)
        {
            PageManager.Instance.SwitchToPage(EViewPage.EmployeeManagerView);
        }
        private void LoginOutAction()
        {
            PageManager.Instance.SwitchToPage(EViewPage.LoginView);
        }

    }
}
