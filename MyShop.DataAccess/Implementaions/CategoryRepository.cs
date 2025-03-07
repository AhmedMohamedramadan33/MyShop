using MyShop.DataAccess.Data;
using MyShop.Entities.IRepositories;
using MyShop.Entities.Models;

namespace MyShop.DataAccess.Implementaions
{
    public class CategoryRepository : GenericRepositories<Category>, ICategoryRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryRepository(AppDbContext context, IUnitOfWork unitOfWork) : base(context)
        {
            _unitOfWork = unitOfWork;
        }

    }
}
