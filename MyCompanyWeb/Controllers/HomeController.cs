using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MyCompanyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string sterm = "", int typeId = 0)
        {
            IEnumerable<Product> products = await _homeRepository.GetProducts(sterm, typeId);
            IEnumerable<ProductType> types = await _homeRepository.Types();
            ProductDisplayModel productModel = new ProductDisplayModel
            {
                Products = products,
                Types = types,
                STerm = sterm,
                TypeId = typeId
            };
            return View(productModel);
        }
        public async Task<IActionResult> Details(int id)
        {
            Product productDetails = await _homeRepository.GetById(id);
            return View(productDetails);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Product productDetails = await _homeRepository.GetById(id);
            return View(productDetails);
        }
        public async Task<IActionResult> Delete(int id)
        {
            Product productDetails = await _homeRepository.GetById(id);
            return View(productDetails);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}