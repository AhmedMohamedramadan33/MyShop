using MyShop.Entities.Models;

namespace MyShop.Entities.IRepositories
{
    public interface IShoppingCardRepository : IGenericRepository<ShoppingCardVM>
    {
        int increasecount(ShoppingCardVM cardVM, int count);
        int decreasecount(ShoppingCardVM cardVM, int count);

    }
}
