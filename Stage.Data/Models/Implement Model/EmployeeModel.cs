using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using Utilities;
using PosStage.MVVM.Models;

namespace PosStage.MVVM.Models.Implement_Model
{
    public class EmployeeModel : Utilities.IFilterableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public GenderModel Gender { get; set; }
        public bool IsDelete { get; set; }
        public RoleViewModel Role { get; set; }
        public int ImageId { get; set;   }
        #region Constructor
        public EmployeeModel()
        {
            _employee = new PB2001_Employee();
        }
        public EmployeeModel(PB2001_Employee item)
        {
            _employee = item;
            Id = item.Id;
            Name = item.Name;
            Account = item.Account;
            Password = item.Password;
            Gender = GetGender(item.Gender);
            IsDelete = item.IsDelete;
        }
        public EmployeeModel(EmployeeModel item)
        {
            _employee = item.Employee;
            Id = item.Id;
            Name = item.Name;
            Account = item.Account;
            Password = item.Password;
            Gender = item.Gender;
            IsDelete = item.IsDelete;
        }
        #endregion
        public PB2001_Employee Employee => GetEmploye();
        private PB2001_Employee _employee;
        #region Gender

        //覺得資料庫為了性別開一個欄位對照有點太浪費，先集中寫在程式的某處
        //TODO 抽去其他地方，並優化每次new的問題
        public static GenderModel GetGender(int id)
        {
            GenderModel gender = new GenderModel();
            if (id == 1)
            {
                gender.Id = 1;
                gender.Name = "男性";
            }
            else if (id == 2)
            {
                gender.Id = 2;
                gender.Name = "女性";
            }
            return gender;
        }
        public static List<GenderModel> GetGenderList()
        {
            List<GenderModel> genderList = new List<GenderModel>();

            GenderModel genderMem = new GenderModel()
            {
                Id = 1,
                Name = "男性",
            };
            GenderModel genderFemale = new GenderModel()
            {
                Id = 2,
                Name = "女性",
            };
            genderList.Add(genderMem);
            genderList.Add(genderFemale);
            return genderList;
        }
        #endregion

        string IFilterableObject.FilterValue => Name;

        private PB2001_Employee GetEmploye()
        {
            _employee.Id = Id;
            _employee.Name = Name;
            _employee.Account = Account;
            _employee.Password = Password;
            _employee.Gender = Gender.Id;
            _employee.IsDelete = IsDelete;
            return _employee;
        }

    }

}
