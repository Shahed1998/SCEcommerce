using Models.BusinessEntities;
using Models.Entities;
using Utility.Helpers;

namespace Manager.Interfaces
{
    public interface ICategoryManager
    {
        Task<List<CategoryDTO>> All();
        Task<PagedList> GetAll(int page, int pageSize, bool getAll = false);
        Task<CategoryDTO> Get(int Id);
        Task<bool> Add(Category entity);
        Task<bool> Remove(CategoryDTO entity);
        Task<bool> RemoveRange(IEnumerable<Category> entities);
        Task<bool> Update(CategoryDTO category);
    }
}
