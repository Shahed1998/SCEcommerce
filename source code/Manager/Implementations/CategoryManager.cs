using DataAccess.Repository.Interfaces;
using Manager.Interfaces;
using Models.Entities;

namespace Manager.Implementations
{
    public class CategoryManager : ICategoryManager
    {

        private readonly ICategoryRepository _categoryRepo;

        public CategoryManager(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public bool Add(Category entity)
        {
            _categoryRepo.Add(entity);
            return _categoryRepo.Save() > 0;
        }

        public Category Get(int Id)
        {
            return _categoryRepo.Get(x => x.Id == Id) ?? new Category();
        }

        public IEnumerable<Category> GetAll()
        {
            return _categoryRepo.GetAll();
        }

        public bool Remove(Category entity)
        {
            _categoryRepo.Remove(entity);
            return _categoryRepo.Save() > 0;
        }

        public bool RemoveRange(IEnumerable<Category> entities)
        {
            _categoryRepo.RemoveRange(entities);
            return _categoryRepo.Save() > 0;
        }

        public bool Update(Category category)
        {
            _categoryRepo.Update(category);
            return _categoryRepo.Save() > 0;
        }
    }
}
