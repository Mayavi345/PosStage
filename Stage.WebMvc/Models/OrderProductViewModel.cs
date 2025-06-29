using PosStage.MVVM.Models.Implement_Model;

namespace Stage.WebMvc.Models;

public class OrderProductViewModel
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public string? CategoryName { get; set; }
    public string ImageSource { get; set; } = string.Empty;

    public static OrderProductViewModel FromProductModel(ProductModel model, string imageSource)
    {
        return new OrderProductViewModel
        {
            ProductId = model.ProductId,
            Name = model.Name,
            Price = model.Price,
            CategoryName = model.Categories?.CategoryName,
            ImageSource = imageSource
        };
    }
}
