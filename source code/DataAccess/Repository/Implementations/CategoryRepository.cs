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

        public void Update(Category category)
        {
            _db.categories.Update(category);
        }

        public bool DisplayOrderAlreadyExist(int displayOrder, int? Id)
        {
            var category = _db.categories.FirstOrDefault(x => x.DisplayOrder == displayOrder);

            if (category != null)
            {
                if (Id is null || Id == 0 || category.Id != Id)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
