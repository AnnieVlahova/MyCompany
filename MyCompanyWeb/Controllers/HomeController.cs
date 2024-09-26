using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyCompanyWeb.Models;
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

        public async Task<IActionResult> Index(string sterm = "",string supplierName = "", int typeId = 0)
        {
            IEnumerable<Product> products = await _homeRepository.GetProducts(sterm,supplierName, typeId);
            IEnumerable<ProductType> types = await _homeRepository.Types();
            IEnumerable<Supplier> suppliers = await _homeRepository.Suppliers();
            ProductDisplayModel productModel = new ProductDisplayModel
            {
                Products = products,
                Types = types,
                Suppliers = suppliers,
                STerm = sterm,
                TypeId = typeId
            };
            return View(productModel);
        }
        public async Task<IActionResult> Details(int id)
        {
            Product productDetails = await _homeRepository.GetProductById(id);
            return View(productDetails);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Product productDetails = await _homeRepository.GetProductById(id);
            return View(productDetails);
        }
        public async Task<IActionResult> Delete(int id)
        {
            Product productDetails = await _homeRepository.GetProductById(id);
            return View(productDetails);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var productDM = new AddProductDisplayModel();
            var prodTypes = await _homeRepository.Types();

            productDM.ProductTypes = new List<SelectListItem>();
            foreach(var prodType in prodTypes)
            {
                productDM.ProductTypes.Add(new SelectListItem { Text = prodType.Name, Value = prodType.Id.ToString() });
            }
            var suppliers = await _homeRepository.Suppliers();
            productDM.Suppliers = new List<SelectListItem>();
            foreach(var supp in suppliers)
            {
                productDM.Suppliers.Add(new SelectListItem { Text = supp.Name, Value = supp.Id.ToString() });
            }

            return View(productDM);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddProductDisplayModel productDM)
        {
            if (!ModelState.IsValid)
            {
                return View(productDM);
            }
            int selectedProductType = Int32.Parse(productDM.ProductTypeSelected);
            if (selectedProductType > -1)
            {
                productDM.ProductTypeId = selectedProductType;
                productDM.ProductType = await _homeRepository.GetProductTypeById(selectedProductType);
            }
            int selectedSupplier = Int32.Parse(productDM.SupplierSelected);
            if (selectedSupplier > -1)
            {
                productDM.SupplierId = selectedSupplier;
                productDM.Supplier = await _homeRepository.GetSupplierById(selectedSupplier);
            }
            var product = new Product
            {
                Name = productDM.Name,
                Description = productDM.Description,
                SerialNumber = productDM.SerialNumber,
                Price = productDM.Price,
                Discount = productDM.Discount,
                InStock = productDM.InStock,
                ProductTypeId = productDM.ProductTypeId,
                ProductType = productDM.ProductType,
                SupplierId = productDM.SupplierId,
                Supplier = productDM.Supplier,
                Image = productDM.Image
            };
            _homeRepository.Add(product);
            return RedirectToAction("Index");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}