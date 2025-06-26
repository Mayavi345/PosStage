using System.ComponentModel.DataAnnotations;

namespace PosStage.MVVM.Models
{
    public class PB1002_CompletedOrdersDetails {
        [Key]
        public int Id { get; set; }
        public int ProductID { get; set; }
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderNumber { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

    }
}
