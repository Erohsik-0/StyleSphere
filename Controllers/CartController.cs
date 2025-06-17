using Microsoft.AspNetCore.Mvc;
using StyleSphere.Helper;
using StyleSphere.Models;
using System.Text.Json;

namespace StyleSphere.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "CartItems";
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<CartController> _logger;

        public CartController(IWebHostEnvironment env, ILogger<CartController> logger)
        {
            _env = env;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var cartItems = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
                var products = LoadProducts();

                var viewModel = new List<CartViewModel>();

                foreach (var item in cartItems)
                {
                    var product = products.FirstOrDefault(p => p.id == item.id);
                    if (product != null)
                    {
                        viewModel.Add(new CartViewModel
                        {
                            id = product.id,
                            name = product.name,
                            buyimage = product.buyimage ?? product.image,
                            price = product.price,
                            size = item.size,
                            quantity = item.quantity
                        });
                    }
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading cart items.");
                TempData["ErrorMessage"] = "Something went wrong while loading your cart.";
                return View(new List<CartViewModel>());
            }
        }

        [HttpPost]
        public IActionResult AddToCart(int id, int quantity, string size)
        {
            try
            {
                var cart = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
                var existing = cart.FirstOrDefault(c => c.id == id && c.size == size);

                if (existing != null)
                    existing.quantity += quantity;
                else
                    cart.Add(new CartItem { id = id, quantity = quantity, size = size });

                HttpContext.Session.SetObject(CartSessionKey, cart);
                _logger.LogInformation("Added item ID {Id} to cart. Total items: {Count}", id, cart.Count);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item {Id} to cart.", id);
                TempData["ErrorMessage"] = "Failed to add item to cart.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult RemoveFromCart(int id, string size)
        {
            try
            {
                var cart = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
                cart = cart.Where(c => c.id != id || c.size != size).ToList();
                HttpContext.Session.SetObject(CartSessionKey, cart);
                _logger.LogInformation("Removed item ID {Id} with size {Size} from cart.", id, size);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item {Id} from cart.", id);
                TempData["ErrorMessage"] = "Failed to remove item from cart.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult UpdateQuantity(int id, string size, int quantity)
        {
            try
            {
                var cart = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
                var item = cart.FirstOrDefault(c => c.id == id && c.size == size);
                if (item != null)
                {
                    item.quantity = quantity;
                    HttpContext.Session.SetObject(CartSessionKey, cart);
                    _logger.LogInformation("Updated quantity for item ID {Id} to {Quantity}", id, quantity);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating quantity for item {Id}", id);
                TempData["ErrorMessage"] = "Failed to update quantity.";
                return RedirectToAction("Index");
            }
        }

        private List<Products> LoadProducts()
        {
            try
            {
                string jsonPath = Path.Combine(_env.WebRootPath, "Data", "products.json");

                if (!System.IO.File.Exists(jsonPath))
                {
                    _logger.LogWarning("Product JSON file not found at {Path}", jsonPath);
                    return new List<Products>();
                }

                string json = System.IO.File.ReadAllText(jsonPath);
                return JsonSerializer.Deserialize<List<Products>>(json) ?? new List<Products>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product list from JSON.");
                return new List<Products>();
            }
        }
    }
}
