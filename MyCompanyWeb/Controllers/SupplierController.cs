using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MyCompanyWeb.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ILogger<SupplierController> _logger;
        private readonly ISupplierRepository _supplierRepository;

        public SupplierController(ILogger<SupplierController> logger, ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string sterm = "", string countryName = "")
        {
            IEnumerable<Supplier> suppliers = await _supplierRepository.GetSuppliers(sterm, countryName);
            SupplierDisplayModel suplierModel = new SupplierDisplayModel
            {
                Suppliers = suppliers
            };
            return View(suplierModel);
        }
        public async Task<IActionResult> Details(int id)
        {
            Supplier supplierDetails = await _supplierRepository.GetById(id);
            return View(supplierDetails);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Supplier supplierDetails = await _supplierRepository.GetById(id);
            return View(supplierDetails);
        }
        public async Task<IActionResult> Delete(int id)
        {
            Supplier supplierDetails = await _supplierRepository.GetById(id);
            return View(supplierDetails);
        }
        [HttpGet]
        public IActionResult Add()
        {
            var supplierDM = new AddSupplierDisplayModel();
            return View(supplierDM);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddSupplierDisplayModel supplierDM)
        {
            if(!ModelState.IsValid)
            {
                return View(supplierDM);
            }
            var supplier = new Supplier
            {
                Name = supplierDM.Name,
                Email = supplierDM.Email,
                Country = supplierDM.Country,
                City = supplierDM.City,
                Address = supplierDM.Address
            };
            _supplierRepository.Add(supplier);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
