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
            try
            {
                // Any logic here can go inside this try block if needed
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Home/Index");
                return RedirectToAction("Error");
            }
        }

        public IActionResult Privacy()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Home/Privacy");
                return RedirectToAction("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            _logger.LogWarning("Rendering error page. Request ID: {RequestId}", requestId);

            return View(new ErrorViewModel
            {
                RequestId = requestId
            });
        }
    }
}