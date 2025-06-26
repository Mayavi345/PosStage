namespace PosStage.MVVM.Models.Implement_Model
{

    public interface IOrderProduct
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double CountPrice => Price * Quantity;
        public int ImageId { get; set; }

    }

}
