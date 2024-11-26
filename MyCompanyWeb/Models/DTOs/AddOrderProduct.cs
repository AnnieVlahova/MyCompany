namespace MyCompanyWeb.Models.DTOs
{
    public class AddOrderProduct
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public double? Price { get; set; }
        public double? Discount { get; set; } = 0;
        public int? Quantity { get; set; } = 0;
        public bool Selected { get; set; }

    }
}
