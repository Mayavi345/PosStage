using System.ComponentModel.DataAnnotations;

namespace PosStage.MVVM.Models
{
    public class PB2002_Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
