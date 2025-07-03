using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StyleSphere.Domain.Interfaces.ICart;
using StyleSphere.Domain.Entities;

using StyleSphere.ViewModels;


namespace StyleSphere.Controllers
{
    
    [Authorize]
    public class CartController : Controller
    {
        
        private readonly UserManager<User> _userManager;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public CartController(UserManager<User> userManager , ICartService cartService , IMapper mapper)
        {
            _userManager = userManager;
            _cartService = cartService;
            _mapper = mapper;
        }

        
        //Viewing the cart for the current user
        public async Task<IActionResult> Index()
        {

            var userId = _userManager.GetUserId(User);
            var cartItems = await _cartService.GetCartAsync(userId);

            var viewModel = _mapper.Map<List<CartViewModel>>(cartItems);
            return View(viewModel);
        }

        //Add to cart for the current user 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId , int quantity=1)
        {
            var userId = _userManager.GetUserId(User);

            

            await _cartService.AddToCartAsync(userId, productId, quantity);
            return RedirectToAction("Index");


        }


        //Remove from cart for the current user
        [HttpPost]
        public async Task<IActionResult> Remove(int cartItemId)
        {

            var userId = _userManager.GetUserId(User);
            try
            {
                await _cartService.RemoveAsync(cartItemId);
            }
            catch(Exception ex)
            {
                               // Handle the exception (e.g., log it, show an error message, etc.)
                ModelState.AddModelError("", "An error occurred while removing the item from the cart.");
                return View("Index");
            }
            return RedirectToAction("Index");
        }

    }
}
