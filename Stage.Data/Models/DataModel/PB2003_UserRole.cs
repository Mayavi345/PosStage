using System.ComponentModel.DataAnnotations;

namespace PosStage.MVVM.Models
{
    public class PB2003_UserRole
    {
        [Key]
        public int UserRoleId { get; set; }
        public int UserId { get; set; }

        public int RoleId { get; set; }
    }
}
