using Microsoft.AspNetCore.Mvc;

namespace StyleSphere.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult Error404()
        {
            return View("404");
        }
    }
}
