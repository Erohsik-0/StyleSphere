using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using StyleSphere.Models.ViewModel;
using StyleSphere.Domain.Entities;
using System.Threading.Tasks;


namespace StyleSphere.Controllers
{
    public class AccountController : Controller
    {
            
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager , SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        //Account login
        [HttpGet]
        public IActionResult Index()
        {
            return View(new AccountCombinedViewModel());
        }

        public IActionResult Login()
        {
            return View();
        }

        //Post: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model , string returnUrl = null)
        {
            if ( !ModelState.IsValid) 
            {
                return View(model);
            }

            var user =  await _userManager.FindByEmailAsync(model.Email);
            if ( user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email , model.Password , false , true);
                if (result.Succeeded)
                {
                    if( !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid login attempt!");
            return View();

        }

        public IActionResult Register()
        {
            return View();
        }

        //Post: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                var msg = "Email already registered. Please log in.";
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, errors = new[] { msg } });
                TempData["RegisterMessage"] = msg;
                return RedirectToAction("Login", "Account");
            }

            var user = new User { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = false, errors = result.Errors.Select(e => e.Description).ToList() });


            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
