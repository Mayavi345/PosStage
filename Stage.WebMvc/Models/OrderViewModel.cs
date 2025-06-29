using System.Collections.Generic;

namespace Stage.WebMvc.Models;

public class OrderViewModel
{
    public List<string> Categories { get; set; } = new();
    public List<OrderProductViewModel> Products { get; set; } = new();
}
