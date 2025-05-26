using DataAccess.Data;
using DataAccess.Repository.Interfaces;

namespace DataAccess.Repository.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _db;
        private ICategoryRepository _categoryRepository;
        private IProductRepository _productRepository;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }

        // Lazy loading repos.... 
        public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_db); 
        public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(_db); 

        public async Task<bool> Save()
        {
            return (await _db.SaveChangesAsync()) > 0;
        }
    }
}
