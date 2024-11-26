using MyCompanyWeb.Data.Enum;

namespace MyCompanyWeb.Models.DTOs
{
    public class OrderDisplayModel
    {
        public string? STerm { get; set; } = "";
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<Customer> Customers { get; set; }

        public IEnumerable<OrderStatus> Statuses { get; set; }

        public double? OverAllPrice { get; set; }
    }
}
