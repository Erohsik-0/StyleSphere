using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using StyleSphere.Domain.Entities;
using StyleSphere.Models.ViewModel;
using StyleSphere.Services;


namespace StyleSphere.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailSender _emailSender;
        private readonly IVerificationStore _verificationStore;
        private readonly IMemoryCache _cache;


        public ProfileController(UserManager<User> userManager, IWebHostEnvironment env, IEmailSender emailSender, IVerificationStore verificationStore, IMemoryCache cache)
        {
            _userManager = userManager;
            _env = env;
            _emailSender = emailSender;
            _verificationStore = verificationStore;
            _cache = cache;
        }

        // GET: Profile
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var viewModel = new ProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                AccessFailedCount = user.AccessFailedCount,
                AvatarUrl = user.AvatarUrl ?? "/images/default-avatar.png",
                Activities = new List<ProfileViewModel.ActivityLog>
                {
                    new() { Time = DateTime.Now.ToString("g"), Action = "Profile viewed" },
                    new() { Time = DateTime.Now.AddMinutes(-10).ToString("g"), Action = "Product added to cart" },
                    new() { Action = "Updated profile information", Time = DateTime.Now.AddHours(-1).ToString("g") }
                }
            };

            return View(viewModel);
        }

        // POST: Profile/Update
        [HttpPost]
        public async Task<IActionResult> Update(ProfileViewModel model)
        {


            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(model.UserName))
                user.UserName = model.UserName;

            if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
                user.PhoneNumber = model.PhoneNumber;

            if (model.AvatarFile != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.AvatarFile.FileName);
                var uploadPath = Path.Combine(_env.WebRootPath, "avatars");
                Directory.CreateDirectory(uploadPath);
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.AvatarFile.CopyToAsync(stream);
                }

                user.AvatarUrl = "/avatars/" + fileName;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new { success = false, errors = result.Errors });

            // Check for AJAX (optional, for best UX)
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = true });

            return RedirectToAction("Index");
        }

        // Optional: Still keep if needed
        [HttpGet]
        public async Task<IActionResult> GetAccessFailedCount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            return Ok(new { count = user.AccessFailedCount });
        }

        [HttpGet]
        public async Task<IActionResult> GetVerificationStatus()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            return Ok(new
            {
                success = true,
                emailConfirmed = user.EmailConfirmed,
                phoneConfirmed = user.PhoneNumberConfirmed
            });
        }


        // POST: Profile/SendEmailVerificationCode
        [HttpPost]
        public async Task<IActionResult> SendEmailVerificationCode([FromBody] EmailRequest req)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null || !user.Email.Equals(req.Email, StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { success = false, message = "Invalid user or email." });
                }

                var code = new Random().Next(100000, 999999).ToString();
                _verificationStore.StoreCode($"email:{user.Id}", code);

                var message = $"Your verification code is <strong>{code}</strong>";
                await _emailSender.SendEmailAsync(req.Email, "Verify your email", message);

                return Ok(new { success = true, message = "Verification code sent to your email." });

            }

            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Failed to send verification code.", error = ex.Message });
            }
        }

        // POST: Profile/VerifyEmailCode
        [HttpPost]
        public async Task<IActionResult> VerifyEmailCOde([FromBody] EmailCodeModel model)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.Email != model.Email)
            {
                return BadRequest(new { success = false, message = "Invalid user or email." });
            }

            var storedCode = _verificationStore.GetCode($"email:{user.Id}");
            if (storedCode != model.Code)
            {
                return BadRequest(new { success = false, message = "Invalid verification code." });
            }

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
            _verificationStore.RemoveCode($"email:{user.Id}");

            return Ok(new { success = true, message = "Email verified successfully." });
        }


        //Data Transfer Object for Email Verification
        public class EmailRequest { public string Email { get; set; } }
        public class EmailCodeModel
        {
            public string Email { get; set; }
            public string Code { get; set; }
        }
    }
}
