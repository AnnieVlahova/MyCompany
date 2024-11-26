using MyCompanyWeb.Data.Enum;

namespace MyCompanyWeb.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrders(OrderStatus orderStatus, string customerName = "");
        Task<Order> GetOrderById(int id);
        Task<Customer> GetCustomerById(int id);
        Task<IEnumerable<Product>> Products(int? productTypeId = null);
        Task<IEnumerable<Customer>> Customers();
        IEnumerable<OrderStatus> OrderStatuses();
        bool Add(Order order);
        bool Delete(Order order);
        bool Edit(Order order);
        bool Save();
        Task<List<ProductType>> GetProductTypes();
    }
}
