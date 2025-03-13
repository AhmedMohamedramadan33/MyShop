using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyShop.Entities.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        [ForeignKey("OrderHeader")]
        public int OrderId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        [ValidateNever]
        public virtual OrderHeader OrderHeader { get; set; }
        [ValidateNever]
        public virtual Product Product { get; set; }
    }
}
