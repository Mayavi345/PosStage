
using System.ComponentModel.DataAnnotations;

namespace PosStage.MVVM.Models
{
    public class PB3001_Product
    {

        [Key]
        public int ProductId { get; set; }
        public double Price { get; set; }
        /// <summary>
        /// 這裡的數量是庫存數量
        /// </summary>
        public int Count { get; set; }
        public int State { get; set; }
        public bool IsDelete { get; set; }
        public int? OrderId { get; set; }
    }
}

