namespace MyCompanyWeb.Models.DTOs
{
    public class SupplierDisplayModel
    {
        public IEnumerable<Supplier> Suppliers { get; set; }
        public string? STerm { get; set; } = "";
        public string? CountryName { get; set; } = "";
    }
}
