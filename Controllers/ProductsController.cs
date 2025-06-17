using Microsoft.AspNetCore.Mvc;
using StyleSphere.Models;
using System.Text.Json;

namespace StyleSphere.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IWebHostEnvironment env, ILogger<ProductsController> logger)
        {
            _env = env;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var products = LoadProducts();
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
                var products = LoadProducts();
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

        [HttpGet]
        public IActionResult GetProducts()
        {
            try
            {
                var products = LoadProducts();
                if (products == null)
                {
                    _logger.LogWarning("Product data not found in GetProducts()");
                    return NotFound("Products.json not found or is invalid.");
                }

                return Json(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to return product list as JSON");
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }

        private List<Products>? LoadProducts()
        {
            try
            {
                string jsonPath = Path.Combine(_env.WebRootPath, "Data", "products.json");

                if (!System.IO.File.Exists(jsonPath))
                {
                    _logger.LogWarning("products.json file not found at path: {Path}", jsonPath);
                    return null;
                }

                string json = System.IO.File.ReadAllText(jsonPath);
                var products = JsonSerializer.Deserialize<List<Products>>(json);

                if (products == null)
                {
                    _logger.LogWarning("Deserialized product list is null.");
                }

                return products;
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "JSON deserialization failed for products.json");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading or parsing products.json");
                return null;
            }
        }
    }
}
