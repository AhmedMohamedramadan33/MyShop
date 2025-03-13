using MyShop.Entities.Models;

namespace MyShop.Entities.IRepositories
{
    public interface IOrderHeaderRepository : IGenericRepository<OrderHeader>
    {
        void UpdateOrderStatus(int id, string? OrderStatus, string? PaymentStatus);
    }
}
