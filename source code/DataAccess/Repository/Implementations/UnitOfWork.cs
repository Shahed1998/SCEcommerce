using DataAccess.Data;
using DataAccess.Repository.Interfaces;

namespace DataAccess.Repository.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _db;
        public ICategoryRepository CategoryRepository { get; private set; } 
        public IProductRepository ProductRepository { get; private set; } 

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            CategoryRepository = new CategoryRepository(_db);
            ProductRepository = new ProductRepository(_db); 
        }

        public async Task<bool> Save()
        {
            return (await _db.SaveChangesAsync()) > 0;
        }
    }
}
