using Models.Entities;

namespace Manager.Interfaces
{
    public interface IProductManager
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Category> Get(int Id);
        Task<bool> Add(Category entity);
        Task<bool> Remove(Category entity);
        Task<bool> RemoveRange(IEnumerable<Category> entities);
        Task<bool> Update(Category category);
    }
}
