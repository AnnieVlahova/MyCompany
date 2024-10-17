using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyCompanyWeb.Models.DTOs
{
    public class EditProductDisplayModel
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? SerialNumber { get; set; }
        public double? Price { get; set; }
        public int? Discount { get; set; } = 0;
        public int? InStock { get; set; }
        public int ProductTypeId { get; set; }
        public ProductType? ProductType { get; set; }

        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public string? Image { get; set; }
        public List<SelectListItem>? ProductTypes { get; set; }
        public string? ProductTypeSelected { get; set; }
        public List<SelectListItem>? Suppliers { get; set; }
        public string? SupplierSelected { get; set; }
    }
}
