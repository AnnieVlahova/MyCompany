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
        public async Task<IActionResult> Index(string customerName, OrderStatus? orderStatus = null)
        {
            IEnumerable<Order> orders = await _orderRepository.GetOrders(customerName, orderStatus);
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
                    Discount = 0, 
                    FinalPrice = 0
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
        
        [HttpGet]
        public async Task<IActionResult> Add(AddOrderDisplayModel orderDM = null, string? productTypeId = null)
        {
            var customers = await _orderRepository.Customers();
            var customersL = new List<SelectListItem>();
            populateSelectList(customers, ref customersL);
            orderDM.Customers = customersL;

            var prodTypes = await _orderRepository.GetProductTypes();
            var productTypesL = new List<SelectListItem>();
            populateSelectList(prodTypes, ref productTypesL);
            orderDM.ProductTypes = productTypesL;
            orderDM.AllProducts = await populateOrderProducts();
            orderDM.SelectedProducts = orderDM.AllProducts;

            orderDM.OrderedOn = DateTime.Now.Date;
            orderDM.DeliveryDate = DateTime.Now.Date;

            return View(orderDM);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddOrderDisplayModel orderDM)
        {
            if(!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ToString());
                }
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
                Subtotal = orderDM.Subtotal,
                IsActive = isActive
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
                        Quantity = aop.Quantity,
                        FinalPrice = aop.FinalPrice
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
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
                return View("Error");
            var prodTypes = await _orderRepository.GetProductTypes();
            var customers = await _orderRepository.Customers();
            var prodTypesL = new List<SelectListItem>();
            var customersL = new List<SelectListItem>();
            populateSelectList(prodTypes, ref prodTypesL);
            populateSelectList(customers, ref customersL, order.CustomerId);
            var orderDM = new EditOrderDisplayModel
            {
                Id = id,
                ProductTypes = prodTypesL,
                Customers = customersL,
                CustomerId = order.CustomerId,
                Customer = order.Customer,
                OrderStatus = order.OrderStatus,
                OrderedOn = order.OrderedOn,
                DeliveryDate = order.DeliveryDate,
                Subtotal = order.Subtotal,
                CustomerSelected = order.Customer.Id.ToString(),
                IsActive = order.IsActive
            };
            orderDM.AllProducts = await populateOrderProducts();
            foreach (OrderProduct op in order.OrderProducts)
            {
                var currSelected = orderDM.AllProducts.Where(sp => sp.ProductId == op.ProductId).FirstOrDefault();
                currSelected.Selected = true;
                currSelected.Quantity = op.Quantity;
                currSelected.Discount = op.Discount;
                currSelected.FinalPrice = op.FinalPrice;
            }
            orderDM.SelectedProducts = orderDM.AllProducts;

            return View(orderDM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditOrderDisplayModel orderDM)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", orderDM);
            }

            var existingOrder = await _orderRepository.GetOrderById(id);
            if (existingOrder == null)
            {
                return View("Error");
            }

            //// Update basic order details
            int selectedCustomer = Int32.Parse(orderDM.CustomerSelected);
            if (selectedCustomer > -1)
            {
                orderDM.CustomerId = selectedCustomer;
                orderDM.Customer = await _orderRepository.GetCustomerById(selectedCustomer);
            }

            existingOrder.CustomerId = orderDM.CustomerId;
            existingOrder.Customer = orderDM.Customer;
            existingOrder.OrderStatus = orderDM.OrderStatus;
            existingOrder.OrderedOn = orderDM.OrderedOn;
            existingOrder.DeliveryDate = orderDM.DeliveryDate;
            existingOrder.Subtotal = orderDM.Subtotal;
            existingOrder.IsActive = orderDM.OrderStatus.Value != OrderStatus.Delivered;

            // Update products only if there are changes
            foreach (AddOrderProduct aop in orderDM.SelectedProducts)
            {
                // Check if the product already exists in the order
                var existingProduct = existingOrder.OrderProducts.FirstOrDefault(op => op.ProductId == aop.ProductId);

                if (aop.Selected && aop.Quantity > 0)
                {
                    if (existingProduct != null)
                    {
                        // Update the existing product
                        existingProduct.Quantity = aop.Quantity;
                        existingProduct.Discount = aop.Discount;
                        existingProduct.FinalPrice = aop.FinalPrice;
                    }
                    else
                    {
                        // Add new product
                        OrderProduct newProduct = new OrderProduct
                        {
                            OrderId = existingOrder.Id,
                            ProductId = aop.ProductId,
                            Product = aop.Product,
                            Price = aop.Price,
                            Discount = aop.Discount,
                            Quantity = aop.Quantity,
                            FinalPrice = aop.FinalPrice
                        };
                        existingOrder.OrderProducts.Add(newProduct);
                    }
                }
                else if (existingProduct != null)
                {
                    // Remove unselected or zero-quantity products
                    existingOrder.OrderProducts.Remove(existingProduct);
                }
            }

            // Save changes to the repository
            _orderRepository.Edit(existingOrder);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null) return View("Error");
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderRepository.GetOrderById(id);

            if (order == null)
            {
                return View("Error");
            }

            _orderRepository.Delete(order);
            return RedirectToAction("Index");
        }

    }
}
