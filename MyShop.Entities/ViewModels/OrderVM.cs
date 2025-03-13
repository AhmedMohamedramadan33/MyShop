using MyShop.Entities.Models;

namespace MyShop.Entities.ViewModels
{
    public class OrderVM
    {
        public OrderHeader OrderHeader { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
