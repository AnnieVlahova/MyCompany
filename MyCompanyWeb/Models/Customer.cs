using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCompanyWeb.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Director { get; set; }
        [Required]
        public string? Country { get; set; }
        [Required]
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? CustomerCode { get; set; }
        [Required]
        public string? CustomerCountryCode { get; set; }
        public List<Order>? Orders { get; set; }

    }
}
