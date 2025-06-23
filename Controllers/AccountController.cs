using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using StyleSphere.Models.ViewModel;
using StyleSphere.Models.User;
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
                var result = await _signInManager.PasswordSignInAsync(user , model.Password , false , false);
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
            return View("Index");

        }


        //Post: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if(!ModelState.IsValid)
            {
                return View("Index");
            }

            var user = new User { Email = model.Email, fullname = model.Name, UserName = model.Name };

            var result = await _userManager.CreateAsync( user , model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            //foreach ( var error in result.Errors)
            //{
            //    ModelState.AddModelError("" , error.Description);
            //}

            return View("Index");

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
