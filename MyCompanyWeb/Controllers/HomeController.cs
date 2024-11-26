using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyCompanyWeb.Models;
using NPOI.SS.Formula.Functions;
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

        public IActionResult Privacy()
        {
            return View();

        }
        private void populateSelectList<T>(IEnumerable<T> list, ref List<SelectListItem> selectList, int selectedVal = -1) where T : IEntity
        {

            foreach (var elem in list)
            {
                selectList.Add(new SelectListItem { Selected = elem.Id == selectedVal, Text = elem.Name, Value = elem.Id.ToString() });
            }
            selectList = selectList.OrderByDescending(p => p.Selected).ToList();
        }
        //private void populateSuppliersAndType(IEnumerable<ProductType> prodTypes, IEnumerable<Supplier> suppliers, ref List<SelectListItem> prodTypesL,
        //    ref List<SelectListItem> suppliersL, int productTypeId = -1, int supplierId = -1)
        //{

        //    foreach (var prodType in prodTypes)
        //    { 
        //        prodTypesL.Add(new SelectListItem { Selected = prodType.Id == productTypeId, Text = prodType.Name, Value = prodType.Id.ToString() });
        //    }
            
        //    foreach (var supp in suppliers)
        //    {
        //        suppliersL.Add(new SelectListItem { Selected = supp.Id == supplierId, Text = supp.Name, Value = supp.Id.ToString() });
        //    }

        //    prodTypesL = prodTypesL.OrderByDescending(p => p.Selected).ToList();
        //    suppliersL = suppliersL.OrderByDescending(s => s.Selected).ToList();
        //}

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var productDM = new AddProductDisplayModel();

            var prodTypes = await _homeRepository.Types();
            var suppliers = await _homeRepository.Suppliers();

            var prodTypesL = new List<SelectListItem>();
            var suppliersL = new List<SelectListItem>();
            //populateSuppliersAndType(prodTypes, suppliers,ref prodTypesL, ref suppliersL);
            populateSelectList(prodTypes, ref prodTypesL);
            populateSelectList(suppliers, ref suppliersL);
            productDM.ProductTypes = prodTypesL;
            productDM.Suppliers = suppliersL;
            
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
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _homeRepository.GetProductById(id);
            if (product == null)
                return View("Error");
            var prodTypes = await _homeRepository.Types();
            var suppliers = await _homeRepository.Suppliers();
            var prodTypesL = new List<SelectListItem>();
            var suppliersL = new List<SelectListItem>();
            populateSelectList(prodTypes, ref prodTypesL, product.ProductTypeId);
            populateSelectList(suppliers, ref suppliersL, product.SupplierId);
            //populateSuppliersAndType(prodTypes, suppliers, ref prodTypesL, ref suppliersL, product.ProductTypeId, product.SupplierId);
            var productDM = new EditProductDisplayModel
            {
                Name = product.Name,
                Description = product.Description,
                SerialNumber = product.SerialNumber,
                Price = product.Price,
                InStock = product.InStock,
                Discount = product.Discount,
                ProductTypeId = product.ProductTypeId,
                ProductType = product.ProductType,
                SupplierId = product.SupplierId,
                Supplier = product.Supplier,
                Suppliers = suppliersL,
                ProductTypes = prodTypesL,
                SupplierSelected = product.Supplier.Name,
                ProductTypeSelected = product.ProductType.Name
            };
            return View(productDM);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditProductDisplayModel productDM)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit product");
                return View("Edit", productDM);
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
                Id = id,
                Name = productDM.Name,
                Description = productDM.Description,
                SerialNumber = productDM.SerialNumber,
                Price = productDM.Price,
                InStock = productDM.InStock,
                Discount = productDM.Discount,
                ProductTypeId = productDM.ProductTypeId,
                ProductType = productDM.ProductType,
                SupplierId = productDM.SupplierId,
                Supplier = productDM.Supplier
            };

            _homeRepository.Edit(product);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _homeRepository.GetProductById(id);
            if (product == null) return View("Error");
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _homeRepository.GetProductById(id);

            if (product == null)
            {
                return View("Error");
            }

            _homeRepository.Delete(product);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}