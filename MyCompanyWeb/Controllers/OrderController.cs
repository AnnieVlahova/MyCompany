using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyCompanyWeb.Data.Enum;

namespace MyCompanyWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderRepository _orderRepository;

        public OrderController(ILogger<OrderController> logger, IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }
        public async Task<IActionResult> Index(OrderStatus orderStatus, string customerName)
        {
            IEnumerable<Order> orders = await _orderRepository.GetOrders(orderStatus, customerName);
            OrderDisplayModel orderDM = new OrderDisplayModel
            {
                Orders = orders
            };
            return View(orderDM);
        }
        private void populateCustomersSelectList(IEnumerable<Customer> customers, ref List<SelectListItem> customersL, int customerId = -1)
        {

            foreach (var cust in customers)
            {
                customersL.Add(new SelectListItem { Selected = cust.Id == customerId, Text = cust.Name, Value = cust.Id.ToString() });
            }
            customersL = customersL.OrderByDescending(s => s.Selected).ToList();
        }
        private async Task<List<AddOrderProduct>> populateOrderProducts(int? productTypeId = null)
        {
            IEnumerable<Product> products = await _orderRepository.Products(productTypeId);
            List<AddOrderProduct> orderProds = new List<AddOrderProduct>();
            foreach(Product prod in products)
            {
                AddOrderProduct op = new AddOrderProduct
                {
                    ProductId = prod.Id,
                    Product = prod, 
                    Price = prod.Price,
                    Selected = false,
                    Quantity = 1,
                    Discount = 0
                };
                orderProds.Add(op);
            }
            return orderProds;

        }
        private void populateSelectList<T>(IEnumerable<T> list, ref List<SelectListItem> selectList, int selectedVal = -1) where T : IEntity
        {

            foreach (var elem in list)
            {
                selectList.Add(new SelectListItem { Selected = elem.Id == selectedVal, Text = elem.Name, Value = elem.Id.ToString() });
            }
            selectList = selectList.OrderByDescending(p => p.Selected).ToList();
        }
        private void populateSeletedProductsList()
        {

        }
        [HttpGet]
        public async Task<IActionResult> Add(AddOrderDisplayModel orderDM = null, string? productTypeId = null)
        {
            var customers = await _orderRepository.Customers();
            var customersL = new List<SelectListItem>();
            populateSelectList(customers, ref customersL);
            orderDM.Customers = customersL;

            var prodTypes = await _orderRepository.GetProductTypes();
            //string selectedProdType = orderDM.SelectedProductType;
            var productTypesL = new List<SelectListItem>();
            populateSelectList(prodTypes, ref productTypesL);
            orderDM.ProductTypes = productTypesL;
            //int? prodTypeId = null;
            //if (selectedProdType != null)
            //{
            //    prodTypeId = Int32.Parse(selectedProdType);
            //}
            orderDM.AllProducts = await populateOrderProducts();
            orderDM.SelectedProducts = orderDM.SelectedProducts;

            return View(orderDM);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddOrderDisplayModel orderDM)
        {
            if(!ModelState.IsValid)
            {
                return View(orderDM);
            }
            int selectedCustomer = Int32.Parse(orderDM.CustomerSelected);
            if(selectedCustomer > -1)
            {
                orderDM.CustomerId = selectedCustomer;
                orderDM.Customer = await _orderRepository.GetCustomerById(selectedCustomer);
            }
            bool isActive = orderDM.OrderStatus.Value != OrderStatus.Delivered;
            var order = new Order
            {
                CustomerId = orderDM.CustomerId,
                Customer = orderDM.Customer,
                OrderStatus = orderDM.OrderStatus,
                OrderedOn = orderDM.OrderedOn,
                DeliveryDate = orderDM.DeliveryDate,
            };
            List<OrderProduct> orderProducts = new List<OrderProduct>();
            foreach(AddOrderProduct aop in orderDM.SelectedProducts)
            {
                if(aop.Selected && aop.Quantity > 0)
                {
                    OrderProduct op = new OrderProduct
                    {
                        OrderId = order.Id,
                        Order = order,
                        ProductId = aop.ProductId,
                        Product = aop.Product,
                        Price = aop.Price,
                        Discount = aop.Discount,
                        Quantity = aop.Quantity
                    };
                    orderProducts.Add(op);
                }
            }
            order.OrderProducts = orderProducts;
            _orderRepository.Add(order);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int id)
        {
            Order orderDetails = await _orderRepository.GetOrderById(id);
            return View(orderDetails);
        }
        public async Task<IActionResult> Delete(int id)
        {
            Order orderDetails = await _orderRepository.GetOrderById(id);
            return RedirectToAction("Index");
        }

    }
}
