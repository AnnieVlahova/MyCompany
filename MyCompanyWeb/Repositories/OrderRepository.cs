using Microsoft.EntityFrameworkCore;
using MyCompanyWeb.Data.Enum;

namespace MyCompanyWeb.Repositories
{

    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Order>> Orders()
        {
            return await _db.Orders.ToListAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            Order order = await _db.Orders.Include(o => o.Customer).Include(o => o.OrderProducts).ThenInclude(op => op.Product).ThenInclude(p => p.ProductType).FirstOrDefaultAsync(o => o.Id == id);
            return order;
        }
        public async Task<Customer> GetCustomerById(int id)
        {
            Customer customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == id);
            return customer;
        }

        public IEnumerable<OrderStatus> OrderStatuses()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> Products(int? productTypeId = null)
        {
            var products = await _db.Products.ToListAsync();
            if (productTypeId != null)
            {
                products = products.Where(p => p.ProductTypeId == productTypeId).ToList();
            }
            return products;
        }
        public async Task<IEnumerable<Customer>> Customers()
        {
            return await _db.Customers.ToListAsync();
        }

        public bool Add(Order order)
        {
            _db.Add(order);
            return Save();
        }
        public bool Delete(Order order)
        {
            _db.Remove(order);
            return Save();
        }
        public bool Edit(Order order)
        {
            _db.Entry(order).State = EntityState.Modified;
            _db.Update(order);
            return Save();
        }
        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved > 0 ? true : false;
        }
        public async Task<IEnumerable<Order>> GetOrders(string customerName, OrderStatus? orderStatus = null)
        {
            var orders = await _db.Orders.Include(o => o.Customer).Include(p => p.OrderProducts).ToListAsync();
            if(orderStatus.HasValue)
                orders = orders.Where(or => or.OrderStatus == orderStatus.Value).ToList();
            if (!String.IsNullOrEmpty(customerName))
                orders = orders.Where(o => o.Customer.Name.StartsWith(customerName)).ToList();

            return orders;

        }

        public async Task<List<ProductType>> GetProductTypes()
        {
            return await _db.ProductTypes.ToListAsync();
        }
    }
}
