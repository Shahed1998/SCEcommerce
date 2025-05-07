using Models.Entities;
using Utility.Helpers;

namespace Manager.Interfaces
{
    public interface IProductManager
    {
        Task<PagedList> GetAll(int page, int pageSize);
        Task<Product> Get(int Id);
        Task<bool> Add(Product product);
        Task<bool> Remove(Product product);
        Task<bool> RemoveRange(IEnumerable<Product> products);
        Task<bool> Update(Product product);
    }
}
