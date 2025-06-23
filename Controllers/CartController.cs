using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StyleSphere.Data;
using StyleSphere.Models.CartEntity;
using StyleSphere.Models.User;
using System.Linq;
using System.Threading.Tasks;

namespace StyleSphere.Controllers
{
    
    [Authorize]
    public class CartController : Controller
    {
        
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public CartController(AppDbContext context , UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            //var userId = _userManager.GetUserId(User);
            //var cartItems = await _context.CartItems.Where(ci => ci.User == userId).Include(ci => ci.Product).ToListAsync();

            return View();
        }
    }
}
