using Microsoft.AspNetCore.Mvc;



namespace StyleSphere.Controllers
{
    public class AccountController : Controller
    {



        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Register(string username , string email , string password)
        //{
        //    if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        //    {
        //        TempData["Error"] = "Please fill all the fields";
        //        return RedirectToAction("Index");
        //    }

        //    var userExists = await _context.User.AnyAsync(u => u.username == username)
        //}
    }
}
