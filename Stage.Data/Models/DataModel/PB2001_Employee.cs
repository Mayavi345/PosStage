using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosStage.MVVM.Models
{
    public class PB2001_Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MaxLength(20)]
        public string Account { get; set; }
        [Required]
        [MaxLength(10)]
        public string Password { get; set; }
        [MaxLength(1)]
        public int Gender { get; set; }
        public bool IsDelete { get; set; }
        public int? ImageId { get; set; }
    }
}

