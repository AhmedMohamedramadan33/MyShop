using MyShop.DataAccess.Data;
using MyShop.Entities.IRepositories;
using System.Collections;

namespace MyShop.DataAccess.Implementaions
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private Hashtable _repositories;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            var key = typeof(T).Name;
            if (!_repositories.ContainsKey(key))
            {
                var ob = new GenericRepositories<T>(_context);
                _repositories.Add(key, ob);
            }
            return _repositories[key] as IGenericRepository<T>;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
