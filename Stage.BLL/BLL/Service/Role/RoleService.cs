using PosStage.MVVM.Models.Implement_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stage.BLL.BLL.Service
{
    public interface IRoleService
    {
        bool IsManager(int roleId);
    }
    public class RoleService : IRoleService
    {
        public RoleService()
        {

        }

        bool IRoleService.IsManager(int roleId)
        {
            if (roleId == 1) {
                return true;
            }
            else
                return false;
        }
    }
    public enum RoleType { 
        Manager=1,
        Staff=2,
        UnDefintion=3,
    } 
}
