using MyShop.DataAccess.Data;
using MyShop.Entities.IRepositories;
using MyShop.Entities.Models;

namespace MyShop.DataAccess.Implementaions
{
    public class UserRepository : GenericRepositories<ApplicationUser>, IUserRepository
    {
        public UserRepository(AppsDbContext context) : base(context)
        {

        }
    }
}
