using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stage.DAL.Repositories.Implement;
using System;
using PosStage.DAL;
using PosStage.MVVM.Models;
using PosStage.MVVM.Models.Implement_Model;
using Utilities.Nlog;

namespace Stage.BLL.BLL.Service
{
    public interface IEmployeeService
    {
        void Add(EmployeeModel newItem);
        EmployeeModel GetItemByAccount(string account);
        List<EmployeeModel> GetItems();
        List<RoleViewModel> GetRoles();
        void Remove(EmployeeModel currentSelected);
        void Update(EmployeeModel currentSelected);
        /// <param name="item"></param>
        /// <param name="IsIncludeSelf">檢查是否為自己(針對更新用)</param>
        /// <returns></returns>
        bool CheckExistAccount(EmployeeModel item, bool IsIncludeSelf = false);
        EmployeeModel CurrentEmployee { get; }
        void SetCurrentEmployee(EmployeeModel employee);
        string FindEmployeeName(int? orderEmployeeId);
    }
    public class EmployeeService : IEmployeeService
    {
        private IEmployeeRepository _repository;
        private EmployeeModel _currentEmployee;

        EmployeeModel IEmployeeService.CurrentEmployee => _currentEmployee;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
            _currentEmployee = new EmployeeModel();
        }

        public List<EmployeeModel> GetItems()
        {
            return _repository.GetItems();
        }
        public EmployeeModel GetItemByAccount(string account)
        {
            return _repository.GetEmployeByAccount(account);
        }

        public void Update(EmployeeModel item)
        {
            _repository.Update(item);
        }

        public void Add(EmployeeModel item)
        {
            _repository.Add(item);
        }

        public void Remove(EmployeeModel item)
        {
            _repository.Remove(item);
        }

        List<RoleViewModel> IEmployeeService.GetRoles()
        {
            return _repository.GetRoles();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="CheckIsSelf">檢查是否為自己(針對更新用)</param>
        /// <returns></returns>
        bool IEmployeeService.CheckExistAccount(EmployeeModel item, bool CheckIsSelf = false)
        {
            var currentAccountList = GetItems();
            if (CheckIsSelf)
            {
                //如果存在的是自己帳號，則回傳不存在其他重複的帳號
                if (currentAccountList.Any(x => x.Id == item.Id))
                {
                    return false;
                }
                else
                {
                    //如果不存在自己帳號，則檢查是否有其他重複的帳號
                    if (currentAccountList.Any(x => x.Account == item.Account))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                //檢查是否有其他重複的帳號
                if (currentAccountList.Any(x => x.Account == item.Account))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        void IEmployeeService.SetCurrentEmployee(EmployeeModel employee)
        {
            _currentEmployee = employee;
        }

        string IEmployeeService.FindEmployeeName(int? orderEmployeeId)
        {
            return FindEmployeeName(orderEmployeeId);
        }
        public string FindEmployeeName(int? orderEmployeeId)
        {
            var employeeList = MainSystemService.Instance.EmployeeService.GetItems();
            var currentEmployee = employeeList.Where(x => x.Id == orderEmployeeId).FirstOrDefault();
            if (currentEmployee != null)
                return currentEmployee.Name;
            else
            {
                LogManagerSingleton.Instance.PrintLog("找不到該員工 orderEmployeeId:" + orderEmployeeId, NLog.LogLevel.Info);
                return string.Empty;
            }
        }
    }
}
