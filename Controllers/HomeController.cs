using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StyleSphere.Models;

namespace StyleSphere.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
                // Any logic here can go inside this try block if needed
                return View();
                
        }

        public IActionResult Privacy()
        {
                return View();
            
        }
    }
}