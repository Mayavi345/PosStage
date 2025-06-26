using PosStage.MVVM.Models.Implement_Model;
using Stage.Common;
using System;

namespace Stage.Backstage.Common
{
    public class RefreshEmployeeObserver : EventObserverBase
    {
        public EmployeeModel EmployeeModel { get; set; }
        public RefreshEmployeeObserver(Action action)
        {
            Action = action;
           // EmployeeModel = employeeModel;
        }
    }
}
