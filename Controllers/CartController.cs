using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StyleSphere.Data;
using StyleSphere.Models.CartEntity;
using StyleSphere.Models.UserEntity;


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

        
        //Viewing the cart for the current user
        public async Task<IActionResult> Index()
        {

            var userId = _userManager.GetUserId(User);
            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .ToListAsync();

            return View(cartItems);
        }

        //Add to cart for the current user 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId , int quantity=1)
        {
            var userId = _userManager.GetUserId(User);

            // Ensure Product Exists
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                TempData["CartError"] = "Product not found. Please select a valid product.";
                return RedirectToAction("Index", "Products");
            }

            var existingItem = await _context.CartItems.FirstOrDefaultAsync( c => c.ProductId == productId && c.UserId == userId);

            if ( existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    ProductId = productId,
                    UserId = userId,
                    Quantity = quantity
                };

                _context.CartItems.Add(cartItem);

            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }


        //Remove from cart for the current user
        [HttpPost]
        public async Task<IActionResult> Remove(int cartItemId)
        {

            var userId = _userManager.GetUserId(User);
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync( c => c.Id == cartItemId && c.UserId == userId);

            if ( cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

    }
}
