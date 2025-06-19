using Microsoft.AspNetCore.Mvc;
using StyleSphere.Data;
using StyleSphere.Models.ProductEntity;

namespace StyleSphere.Controllers
{
    public class CartController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;

        public CartController(IWebHostEnvironment env, AppDbContext context)
        {
            _env = env;
            _context = context;
        }

        public IActionResult Index()
        {
            
            return View(_context.Products.ToList());
        }
    }
}
