using Models.BusinessEntities;
using Models.Entities;
using Utility.Helpers;

namespace Manager.Interfaces
{
    public interface ICategoryManager
    {
        Task<List<CategoryVM>> All();
        Task<PagedList> GetAll(int page, int pageSize, bool getAll = false);
        Task<CategoryVM> Get(int Id);
        Task<bool> Add(Category entity);
        Task<bool> Remove(CategoryVM entity);
        Task<bool> RemoveRange(IEnumerable<Category> entities);
        Task<bool> Update(CategoryVM category);
        bool DisplayOrderAlreadyExist(int displayOrder, int? Id);
    }
}
