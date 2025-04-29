using Models.Entities;
using Utility.Helpers;

namespace Manager.Interfaces
{
    public interface ICategoryManager
    {
        Task<PagedList> GetAll(int page, int pageSize);
        Task<Category> Get(int Id);
        Task<bool> Add(Category entity);
        Task<bool> Remove(Category entity);
        Task<bool> RemoveRange(IEnumerable<Category> entities);
        Task<bool> Update(Category category);
    }
}
