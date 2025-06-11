using Microsoft.AspNetCore.Mvc;

namespace StyleSphere.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
