using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PosStage.DAL;
using PosStage.MVVM.Models;
using PosStage.MVVM.Models.Implement_Model;
using Utilities.Nlog;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Stage.DAL.Repositories.Implement
{
    public interface IEmployeeRepository
    {
        void Add(EmployeeModel item);
        EmployeeModel GetEmployeByAccount(string account);
        List<EmployeeModel> GetItems();
        List<RoleViewModel> GetRoles();
        void Remove(EmployeeModel item);
        void Update(EmployeeModel item);
    }
    public class EmployeeRepositoryEF : IEmployeeRepository
    {
        private PosDbContext _dbContext;
        public EmployeeRepositoryEF()
        {
            _dbContext = new PosDbContext();
        }

        void IEmployeeRepository.Add(EmployeeModel item)
        {
            try
            {
                ///儲存Employe
                item.Gender = item.Gender;
                _dbContext.PB2001_Employee.Add(item.Employee);
                _dbContext.SaveChanges();
                ///儲存UserRole
                ///Id 是資料庫給的，需要再取得一次
                var currentEmployee = _dbContext.PB2001_Employee.Where(x => x.Account == item.Account).FirstOrDefault();
                PB2003_UserRole userRole = new PB2003_UserRole()
                {
                    UserId = currentEmployee.Id,
                    RoleId = item.Role.RoleId,
                };
                _dbContext.PB2003_UserRole.Add(userRole);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                throw new Exception();
            }
        }

        EmployeeModel IEmployeeRepository.GetEmployeByAccount(string account)
        {
            try
            {

                var query = from employees in _dbContext.PB2001_Employee
                            join userRole in _dbContext.PB2003_UserRole
                            on employees.Id equals userRole.UserId
                            join role in _dbContext.PB2002_Role
                            on userRole.RoleId equals role.RoleId
                            where employees.Account == account
                            where employees.IsDelete == false
                            select MapEmployeeModel(employees, role);
                var result = query.FirstOrDefault();
                return result;
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                throw new Exception();
            }

        }

        private static EmployeeModel MapEmployeeModel(PB2001_Employee employees, PB2002_Role role)
        {
            return new EmployeeModel
            {
                Id = employees.Id,
                Name = employees.Name,
                Account = employees.Account,
                Password = employees.Password,
                Gender = EmployeeModel.GetGender(employees.Gender),
                IsDelete = employees.IsDelete,
                ImageId = employees.ImageId ?? 0,
                Role = new RoleViewModel()
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName
                }
            };
        }

        List<EmployeeModel> IEmployeeRepository.GetItems()
        {
            try
            {
                var query = from employees in _dbContext.PB2001_Employee
                            join userRole in _dbContext.PB2003_UserRole
                            on employees.Id equals userRole.UserId
                            join role in _dbContext.PB2002_Role
                            on userRole.RoleId equals role.RoleId
                            where employees.IsDelete == false
                            select MapEmployeeModel(employees, role);

                var results = query.ToList();
                return results;
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                throw new Exception();
            }
        }

        List<RoleViewModel> IEmployeeRepository.GetRoles()
        {
            try
            {
                var tempList = _dbContext.PB2002_Role.ToList();
                List<RoleViewModel> roleViewModelList = new List<RoleViewModel>();
                foreach (var item in tempList)
                {
                    RoleViewModel roleViewModel = new RoleViewModel(item);
                    roleViewModelList.Add(roleViewModel);
                }
                return roleViewModelList;
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                throw new Exception();
            }
        }

        void IEmployeeRepository.Remove(EmployeeModel item)
        {
            try
            {
                var currentItem = _dbContext.PB2001_Employee.Where(x => x.Id == item.Employee.Id).FirstOrDefault();
                currentItem.IsDelete = true;
                _dbContext.Update(currentItem);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                throw new Exception();

            }
        }

        void IEmployeeRepository.Update(EmployeeModel item)
        {
            try
            {
                PB2001_Employee? employeItem = _dbContext.PB2001_Employee.Where(x => x.Id == item.Employee.Id).FirstOrDefault();
                MapEmplouee(employeItem, item.Employee);

                PB2003_UserRole? userRoleIdItem = _dbContext.PB2003_UserRole.Where(x => x.UserId == item.Employee.Id).FirstOrDefault();
                userRoleIdItem.RoleId = item.Role.RoleId;

                _dbContext.Update(employeItem);
                _dbContext.Update(userRoleIdItem);

                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                throw new Exception();

            }
        }
        private void MapEmplouee(PB2001_Employee oldValue, PB2001_Employee mewValue)
        {
            oldValue.Name = mewValue.Name;
            oldValue.Account = mewValue.Account;
            oldValue.Password = mewValue.Password;
            oldValue.Gender = mewValue.Gender;
            oldValue.IsDelete = mewValue.IsDelete;
            oldValue.ImageId = mewValue.ImageId;
        }
    }
}
