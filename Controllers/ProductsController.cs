using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;

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
            return View();
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            string jsonPath = Path.Combine(_env.WebRootPath, "Data", "products.json");

            if (!System.IO.File.Exists(jsonPath))
                return NotFound("products.json not found.");

            string json = System.IO.File.ReadAllText(jsonPath);
            return Content(json, "application/json");
        }
    }
}
