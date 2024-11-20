using Microsoft.AspNetCore.Mvc;

namespace ProniaProject.Conrollers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
