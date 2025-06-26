using System.ComponentModel.DataAnnotations;

namespace PosStage.MVVM.Models
{
    public class PB3003_Categories
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public bool IsEnable { get; set; }
        public bool IsDelete { get; set; }
        public int? OrderId { get; set; }
    }
}
