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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if ( !ModelState.IsValid) 
            {
                return PartialView("_LoginForm", model);
            }

            var user =  await _userManager.FindByEmailAsync(model.Email);
            if ( user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user , model.Password , false , false);
                if (result.Succeeded)
                {
                    return Json( new { success = true , message = "Login successful" , redirectUrl = Url.Action("Index" , "Home")});
                }
            }

            return Json(new { success = false, message = "Invalid usernmae or password" });
        }


        //Post: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if(!ModelState.IsValid)
            {
                return PartialView("_RegisterForm" , model);
            }

            var user = new User { Email = model.Email, fullname = model.Name };

            var result = await _userManager.CreateAsync( user , model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Json(new { success = true , message="Registration Successful" , redirectUrl = Url.Action("Index" , "Home")});
            }

            var errors = string.Join(" " , result.Errors.Select(e => e.Description));
            return Json(new { success = false , message = errors});

        }


    }
}
