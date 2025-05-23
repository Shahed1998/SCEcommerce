using Models.BusinessEntities;
using Models.Entities;
using Utility.Helpers;

namespace Manager.Interfaces
{
    public interface IProductManager
    {
        Task<PagedList> GetAll(int page, int pageSize);
        Task<ProductDTO> Get(int Id);
        Task<ProductDTO> GetWithCategory(int Id);
        Task<bool> Add(ProductDTO product);
        Task<bool> Remove(Product product);
        Task<bool> RemoveRange(IEnumerable<Product> products);
        Task<bool> Update(Product product);
    }
}
