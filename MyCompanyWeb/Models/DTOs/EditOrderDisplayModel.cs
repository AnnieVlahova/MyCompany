using Microsoft.AspNetCore.Mvc.Rendering;
using MyCompanyWeb.Data.Enum;

namespace MyCompanyWeb.Models.DTOs
{
    public class EditOrderDisplayModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public DateTime OrderedOn { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveryDate { get; set; }
        public bool? IsActive { get; set; } = false;
        public List<SelectListItem>? Customers { get; set; }
        public List<AddOrderProduct>? AllProducts { get; set; }
        public List<AddOrderProduct>? SelectedProducts { get; set; }
        public string? CustomerSelected { get; set; }

        public List<SelectListItem>? ProductTypes { get; set; }
        public string? SelectedProductType { get; set; }

        public double? Subtotal { get; set; }
    }
}
