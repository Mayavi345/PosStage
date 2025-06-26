using System;
using System.ComponentModel.DataAnnotations;

namespace PosStage.MVVM.Models
{
    public class PB1001_CompletedOrders
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        [Key]
        public string OrderNumber { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Timestamp { get; set; }
        public int? OrderEmployeeId { get; set; }
    }
}
