using GalaSoft.MvvmLight;
using Stage.Backstage.Common;
using Stage.BLL.BLL;
using System;

namespace Stage.Backstage.ViewModel
{
    public class RuleManager : IRuleManager
    {
        private Action<bool> _action;
        public RuleManager()
        {
        }
        void IRuleManager.RegisterObserver(Action<bool> action)
        {
            MainDataCenter.Instance.RefreshNavBar.RegisterObserver(new RefreshEmployeeObserver(UpdateCurrentRole));
            _action = action;
        }
        private void UpdateCurrentRole()
        {
            var currentRole = MainSystemService.Instance.EmployeeService.CurrentEmployee.Role;
            var state = MainSystemService.Instance.RoleService.IsManager(currentRole.RoleId);
            _action?.Invoke(state);
        }
    }

}
