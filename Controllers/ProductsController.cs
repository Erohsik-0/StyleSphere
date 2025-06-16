using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using StyleSphere.Models;
using System.Text.Json;


namespace StyleSphere.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public ProductsController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Details(int id)
        {

            var products = LoadProducts();
            if (products == null)
            {
                return NotFound("Product data not found");
            }

            //Using a dictionary for fast lookup by ID
            var productDictionary = new Dictionary<int, Products>();
            foreach (var p in products)
            {
                productDictionary[p.id] = p;
            }

            if (!productDictionary.TryGetValue(id, out var product))
            {
                return NotFound($"Product with ID {id} not found...");
            }

            return View(product); //Passing product model to the view
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            //string jsonPath = Path.Combine(_env.WebRootPath, "Data", "products.json");

            //if (!System.IO.File.Exists(jsonPath))
            //    return NotFound("products.json not found.");

            //string json = System.IO.File.ReadAllText(jsonPath);
            //return Content(json, "application/json");

            var products = LoadProducts();
            if (products == null)
            {
                return NotFound("Products.json not found");
            }

            return Json(products); // Returning as JSON using C# List<Product> directly
        }

        private List<Products>? LoadProducts()
        {
            string jsonPath = Path.Combine(_env.WebRootPath, "Data", "products.json");

            if ( !System.IO.File.Exists(jsonPath))
            {
                return null;
            }

            string json = System.IO.File.ReadAllText(jsonPath);
            return JsonSerializer.Deserialize<List<Products>>(json);
        }

    }
}
