using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCompanyWeb.Models
{
    [Table("ProductType")]
    public class ProductType : IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string? Name { get; set; }
        public List<Product>? Products { get; set; }

    }
}
