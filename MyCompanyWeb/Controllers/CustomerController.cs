using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MyCompanyWeb.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ILogger<CustomerController> logger, ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string sterm = "", string cityName = "")
        {
            IEnumerable<Customer> customers = await _customerRepository.GetCustomers(sterm, cityName);
            CustomerDisplayModel customerModel = new CustomerDisplayModel
            {
                Customers = customers
            };
            return View(customerModel);
        }
        public async Task<IActionResult> Details(int id)
        {
            Customer customerDetails = await _customerRepository.GetById(id);
            return View(customerDetails);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Customer customerDetails = await _customerRepository.GetById(id);
            return View(customerDetails);
        }
        public async Task<IActionResult> Delete(int id)
        {
            Customer customerDetails = await _customerRepository.GetById(id);
            return View(customerDetails);
        }
        [HttpGet]
        public IActionResult Add()
        {
            var customerDM = new AddCustomerDisplayModel();
            return View(customerDM);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddCustomerDisplayModel customerDM)
        {
            if(!ModelState.IsValid)
            {
                return View(customerDM);
            }
            var customer = new Customer
            {
                Name = customerDM.Name,
                Email = customerDM.Email,
                Director = customerDM.Director,
                Country = customerDM.Country,
                City = customerDM.City,
                Address = customerDM.Address,
                CustomerCode = customerDM.CustomerCode,
                CustomerCountryCode = customerDM.CustomerCountryCode
            };
            _customerRepository.Add(customer);
            return RedirectToAction("Index");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}

