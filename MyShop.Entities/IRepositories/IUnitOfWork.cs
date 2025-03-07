namespace MyShop.Entities.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {


        public IGenericRepository<T> GetRepository<T>() where T : class;
        public int SaveChanges();

    }
}
