using GalaSoft.MvvmLight;
using Stage.BLL.BLL;
using Stage.Presentation.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PosStage.MVVM.ViewModel
{
    public class NavBarViewModel : ObservableObject
    {
        public NavBarViewModel()
        {
            MainDataCenter.Instance.RefreshNavBar.RegisterObserver(new RefreshEmployeeObserver(UpdateMember));
        }

        public void UpdateMember()
        {
            var currentEmployee = MainSystemService.Instance.EmployeeService.CurrentEmployee;
            EmployeeName = currentEmployee.Name;
            EmployeeGender = currentEmployee.Gender.Name;
            EmployeeImage = GetEmployeeImage(currentEmployee.ImageId);
        }

        private ImageSource GetEmployeeImage(int id)
        {
            return ImageSourceProcess.Instance.GetImage(id);
        }
        private ImageSource _employeeImage;
        public ImageSource EmployeeImage
        {
            get => _employeeImage;
            set
            {
                _employeeImage = value;
                RaisePropertyChanged(nameof(EmployeeImage));
            }
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

    }
}
