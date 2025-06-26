using System.Data;
using System.Xml.Linq;

namespace PosStage.MVVM.Models.Implement_Model
{
    public class RoleViewModel : IComboBoxGenericItem<RoleViewModel>
    {
        private readonly PB2002_Role _role;

        public int RoleId { get; set; }

        public string RoleName { get; set; }


        #region IComboBoxGenericItem
        public string DisplayValue { get => RoleName; set { RoleName = value; } }
        public RoleViewModel Value { get => this; set { } }
        #endregion

        public RoleViewModel()
        {
            _role = new PB2002_Role();
            _role.RoleId = RoleId;
            _role.RoleName = RoleName;
        }
        public RoleViewModel(PB2002_Role role)
        {
            this._role = role;
            RoleId = role.RoleId;
            RoleName = role.RoleName;
        }
        public PB2002_Role ConvertRole()
        {
            return _role;
        }
    }
}
