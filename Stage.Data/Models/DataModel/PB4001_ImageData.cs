using System.ComponentModel.DataAnnotations;

namespace PosStage.MVVM.Models
{
    public class PB4001_ImageData {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] Image { get; set; }
        public string? FileExtension { get; set; }
        public decimal Size { get; set; }

    }
}
