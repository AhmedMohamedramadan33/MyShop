using MyShop.DataAccess.Data;
using MyShop.Entities.IRepositories;
using MyShop.Entities.Models;

namespace MyShop.DataAccess.Implementaions
{

    public class ShoppingCardRepository : GenericRepositories<ShoppingCardVM>, IShoppingCardRepository
    {

        public ShoppingCardRepository(AppsDbContext context) : base(context)
        {

        }

        public int decreasecount(ShoppingCardVM cardVM, int count)
        {
            cardVM.Count = count;
            return cardVM.Count;
        }

        public int increasecount(ShoppingCardVM cardVM, int count)
        {
            cardVM.Count += count;
            return cardVM.Count;
        }
    }
}