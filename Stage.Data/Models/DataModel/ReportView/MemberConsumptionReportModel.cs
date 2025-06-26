using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosStage.MVVM.Models
{
    /// <summary>
    /// 會員消費紀錄報表的顯示資料
    /// </summary>
    public class MemberConsumptionReportModel
    {
        [Key]
        public string MemberName { get; set; }
        public string MemberGender { get; set; }
        public string ConsumptionName { get; set; }
        public double OrderTotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
