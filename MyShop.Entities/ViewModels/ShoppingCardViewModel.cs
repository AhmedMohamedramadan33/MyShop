using MyShop.Entities.Models;

namespace MyShop.Entities.ViewModels
{
    public class ShoppingCardViewModel
    {
        public IEnumerable<ShoppingCardVM> cards { get; set; }
        public decimal Total { get; set; }
        public string? Address { get; set; }
        public string? phonenumber { get; set; }
    }
}
