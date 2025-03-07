using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyShop.Entities.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public decimal Price { get; set; }
        [ForeignKey("Category")]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; } = default;
    }
}
