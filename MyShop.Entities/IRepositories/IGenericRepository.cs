using System.Linq.Expressions;

namespace MyShop.Entities.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null, string? IncludeWord = null);
        T GetFirstOrDefault(Expression<Func<T, bool>>? predicate = null, string? IncludeWord = null);
        T GetById(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
