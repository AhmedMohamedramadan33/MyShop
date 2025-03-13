using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyShop.Entities.Models
{
    public class ShoppingCardVM
    {
        [Key]
        public int ShoppingCardId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [ValidateNever]
        public Product Product { get; set; }
        [Range(1, 20, ErrorMessage = "Please enter a value btween 1 and 20")]
        public int Count { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

    }
}
