namespace MyCompanyWeb.Models.DTOs
{
    public class ProductDisplayModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<ProductType> Types { get; set; }
        public IEnumerable<Supplier> Suppliers { get; set; }
        public string? STerm { get; set; } = "";
        public int TypeId { get; set; } = 0;
    }
}
