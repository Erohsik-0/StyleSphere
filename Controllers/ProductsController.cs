using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StyleSphere.Domain.Entities;
using StyleSphere.Domain.Interfaces.IProduct;
using StyleSphere.Models.ViewModel;
using System.Threading.Tasks;

namespace StyleSphere.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IWebHostEnvironment _env;

        private readonly IProductService _productService;

        private readonly IMapper _mapper;

        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IWebHostEnvironment env, IProductService productService , IMapper mapper , ILogger<ProductsController> logger)
        {
            _env = env;
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {

                //Domain -> Data Access -> Service -> Controller -> ViewModel -> View
                var products = await _productService.GetAllProductsAsync();
                if (products == null)
                {
                    _logger.LogWarning("Product list is null in Index()");
                    return NotFound("Product data could not be loaded.");
                }

                // Map the product list to the view model - clean , secure and efficient way to handle data transfer
                var viewModel = _mapper.Map<List<ProductViewModel>>(products);
                return View(viewModel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product list in Index()");
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                //Domain -> Data Access -> Service -> Controller -> ViewModel -> View
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    _logger.LogInformation("Product with ID {Id} not found", id);
                    return NotFound($"Product with ID {id} not found.");
                }

                var viewModel = _mapper.Map<ProductViewModel>(product);
                return View(viewModel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product details for ID {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }


    }
}
