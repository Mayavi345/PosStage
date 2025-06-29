using Microsoft.AspNetCore.Mvc;

namespace Stage.WebMvc.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
