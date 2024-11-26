using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCompanyWeb.Models
{
    [Table("Product")]
    public class Product : IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? SerialNumber { get; set; }
        [Required]
        public double? Price { get; set; }
        public int? Discount { get; set; } = 0;
        public int? InStock { get; set; }
        [Required]
        [ForeignKey("ProductType")]
        public int ProductTypeId { get; set; }
        public ProductType? ProductType { get; set; }

        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public string? Image { get; set; }
        public List<OrderProduct>? OrderProducts { get; set; }

    }
}
