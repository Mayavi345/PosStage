using System;
using Microsoft.AspNetCore.Mvc;
using Stage.BLL.BLL;
using Stage.WebMvc.Models;

namespace Stage.WebMvc.Controllers;

public class ProductController : Controller
{
    public IActionResult List()
    {
        var products = MainSystemService.Instance.ProductService.GetAllProducts(true);
        return View(products);
    }

    public IActionResult Order()
    {
        var productModels = MainSystemService.Instance.ProductService.GetAllProducts(true);
        var categories = MainSystemService.Instance.ProductService.GetCategorieNameList();
        var productVms = new List<OrderProductViewModel>();
        foreach(var p in productModels)
        {
            string img = string.Empty;
            var imgResp = MainSystemService.Instance.ProductService.GetImage(p.ImageId);
            if(imgResp.IsSuccess && imgResp.Data?.Image != null)
            {
                var ext = imgResp.Data.FileExtension ?? "png";
                var base64 = Convert.ToBase64String(imgResp.Data.Image);
                img = $"data:image/{ext.TrimStart('.')};base64,{base64}";
            }
            productVms.Add(OrderProductViewModel.FromProductModel(p, img));
        }
        var vm = new OrderViewModel
        {
            Products = productVms,
            Categories = categories
        };
        return View(vm);
    }
}
