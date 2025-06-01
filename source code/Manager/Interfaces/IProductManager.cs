using Models.BusinessEntities;
using Models.Entities;
using Utility.Helpers;

namespace Manager.Interfaces
{
    public interface IProductManager
    {
        Task<PagedList> GetAll(int page, int pageSize);
        Task<ProductVM> Get(int Id);
        Task<ProductVM> GetWithCategory(int Id);
        Task<bool> Add(ProductVM product);
        Task<bool> Remove(int Id);
        Task<bool> RemoveRange(IEnumerable<Product> products);
        Task<bool> Update(ProductVM product);
    }
}
