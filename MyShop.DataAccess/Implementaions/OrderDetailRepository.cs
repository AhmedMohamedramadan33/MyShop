using MyShop.DataAccess.Data;
using MyShop.Entities.IRepositories;
using MyShop.Entities.Models;

namespace MyShop.DataAccess.Implementaions
{
	public class OrderDetailRepository : GenericRepositories<OrderDetail>, IOrderDetailRepository
	{
		public OrderDetailRepository(AppsDbContext context) : base(context)
		{

		}
	}
}
