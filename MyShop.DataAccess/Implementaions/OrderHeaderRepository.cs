using MyShop.DataAccess.Data;
using MyShop.Entities.IRepositories;
using MyShop.Entities.Models;

namespace MyShop.DataAccess.Implementaions
{
    public class OrderHeaderRepository : GenericRepositories<OrderHeader>, IOrderHeaderRepository
    {
        private readonly AppsDbContext _context;
        public OrderHeaderRepository(AppsDbContext context) : base(context)
        {
            _context = context;
        }
        public void UpdateOrderStatus(int id, string? OrderStatus, string? PaymentStatus)
        {
            var order = _context.orderHeaders.FirstOrDefault(u => u.Id == id);
            if (order != null)
            {
                order.OrderStatus = OrderStatus;
                if (PaymentStatus != null)
                {
                    order.PaymentStatus = PaymentStatus;
                }
            }
        }
    }
}
