using System.ComponentModel.DataAnnotations;

namespace PosStage.MVVM.Models
{
    public class PB3002_ProductDetail
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int? ImageId { get; set; }
    }
}
