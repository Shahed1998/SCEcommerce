using Models.Entities;

namespace Manager.Interfaces
{
    public interface ICategoryManager
    {
        IEnumerable<Category> GetAll();
        Category Get(int Id);
        bool Add(Category entity);
        bool Remove(Category entity);
        bool RemoveRange(IEnumerable<Category> entities);
        bool Update(Category category);
    }
}
