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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
