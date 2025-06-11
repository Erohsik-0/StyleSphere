using Microsoft.AspNetCore.Mvc;

namespace StyleSphere.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
