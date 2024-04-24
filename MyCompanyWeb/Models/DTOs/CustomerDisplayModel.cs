namespace MyCompanyWeb.Models.DTOs
{
    public class CustomerDisplayModel
    {
        public IEnumerable<Customer> Customers { get; set; }
        public string? STerm { get; set; } = "";
        public string? CityName { get; set; } = "";
    }
}
