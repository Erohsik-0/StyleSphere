using Microsoft.AspNetCore.Mvc;
using StyleSphere.Data;
using StyleSphere.Models.ProductEntity;

namespace StyleSphere.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ProductsController> _logger;
        private readonly AppDbContext _context;

        public ProductsController(IWebHostEnvironment env, ILogger<ProductsController> logger, AppDbContext context)
        {
            _env = env;
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                var products = _context.Products.ToList();
                if (products == null)
                {
                    _logger.LogWarning("Product list is null in Index()");
                    return NotFound("Product data could not be loaded.");
                }
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product list in Index()");
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                var products = _context.Products.ToList();
                if (products == null)
                {
                    _logger.LogWarning("Product data is null in Details()");
                    return NotFound("Product data not found.");
                }

                var product = products.FirstOrDefault(p => p.id == id);
                if (product == null)
                {
                    _logger.LogInformation("Product with ID {Id} not found", id);
                    return NotFound($"Product with ID {id} not found.");
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product details for ID {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.id == id);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {Id} not found in AddToCart()", id);
                    return NotFound($"Product with ID {id} not found.");
                }

                product.isAddedToCart = true;
                _context.SaveChanges();

                return RedirectToAction("Index" , "Cart");
                //return Ok($"Product with ID {id} added to cart.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product with ID {Id} to cart", id);
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult RemoveFromCart(int id)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.id == id);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {Id} not found in RemoveFromCart()", id);
                    return NotFound($"Product with ID {id} not found.");
                }

                product.isAddedToCart = false;
                _context.SaveChanges();

                return RedirectToAction("Index", "Cart");
                //return Ok($"Product with ID {id} removed from cart.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing product with ID {Id} from cart", id);
                return RedirectToAction("Error", "Home");
            }
        }

    }
}
