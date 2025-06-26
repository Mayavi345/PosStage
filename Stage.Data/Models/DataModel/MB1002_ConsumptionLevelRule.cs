using System.ComponentModel.DataAnnotations;

namespace PosStage.MVVM.Models
{
    public class MB1002_ConsumptionLevelRule
    {
        [Key]
        public int MemberLevel { get; set; }
        public double ConsumptionPrice { get; set; }
        public string Name { get; set; }
    }
}
