using MyCompanyWeb.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCompanyWeb.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public DateTime? OrderedOn { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveryDate { get; set; }
        public bool? IsActive { get; set; } = false;
        public double? Subtotal { get; set; }

        public List<OrderProduct>? OrderProducts { get; set; }

    }
}
