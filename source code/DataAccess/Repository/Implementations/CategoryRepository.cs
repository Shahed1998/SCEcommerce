using DataAccess.Data;
using DataAccess.Repository.Interfaces;
using Models.Entities;

namespace DataAccess.Repository.Implementations
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {

        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public int Save()
        {
            return _db.SaveChanges();
        }

        public void Update(Category category)
        {
            _db.categories.Update(category);
        }
    }
}
