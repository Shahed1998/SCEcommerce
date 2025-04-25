using DataAccess.Data;
using DataAccess.Repository.Interfaces;
using Models.Entities;

namespace DataAccess.Repository.Implementations
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public void Update(Product prod)
        {
            _db.products.Update(prod);
        }
    }
}
