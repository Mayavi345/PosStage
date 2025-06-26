using System.ComponentModel.DataAnnotations;

namespace PosStage.MVVM.Models
{
    public class MB1003_MemberOrderInfo
    {
        [Key]
        public string OrderNumber { get; set; }
        public int MemberId { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
