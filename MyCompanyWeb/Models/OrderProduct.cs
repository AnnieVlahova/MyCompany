using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCompanyWeb.Models
{
    [Table("OrderProduct")]
    public class OrderProduct
    {
        [Key]
        public int Id { get; set; }
        [Required]

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        [Required]

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        [Required]
        public double? Price { get; set; }
        public double? Discount { get; set; } = 0;
        [Required]
        public int? Quantity { get; set; }

    }
}
