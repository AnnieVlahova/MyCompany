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

        [ForeignKey("OrderStatus")]
        public int? OrderStatusId { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public DateTime OrderedOn { get; set; } = DateTime.UtcNow;
        public DateTime? DeliberyDate { get; set; }
        public bool? IsActive { get; set; } = false;
        [Required]
        public bool IsAccepted { get; set; } = false;

        public List<OrderProduct>? OrderProducts { get; set; }

    }
}
