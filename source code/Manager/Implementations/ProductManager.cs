using DataAccess.Repository.Interfaces;
using Manager.Interfaces;
using Microsoft.Data.SqlClient;
using Models.Entities;
using Utility.Helpers;

namespace Manager.Implementations
{
    public class ProductManager : IProductManager
    {

        private readonly IUnitOfWork _uow;

        public ProductManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> Add(Product product)
        {
            await _uow.ProductRepository.Add(product);
            return await _uow.Save();
        }

        public async Task<Product> Get(int Id)
        {
            return (await _uow.ProductRepository.Get(x => x.Id == Id)) ?? new Product();
        }

        public async Task<PagedList> GetAll(int page, int pageSize)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@PAGENUMBER", System.Data.SqlDbType.Int) { Value = page },
                new SqlParameter("@PAGESIZE", System.Data.SqlDbType.Int) { Value = pageSize },
            };

            string sql1 = "SELECT * FROM Product ORDER BY ID OFFSET @PAGESIZE*(@PAGENUMBER-1) ROWS" +
                " FETCH NEXT @PAGESIZE ROWS ONLY";

            var products = _uow.ProductRepository.ExecuteQuery(sql1, sqlParams);

            string sql2 = "SELECT COUNT(1) FROM Product";

            var totalCount = await _uow.ProductRepository.ExecuteScalar<int>(sql2, sqlParams);

            var result = new PagedList(page, pageSize, totalCount);

            result.products = products;

            return result;
        }

        public async Task<bool> Remove(Product product)
        {
            _uow.ProductRepository.Remove(product);
            return await _uow.Save();
        }

        public async Task<bool> RemoveRange(IEnumerable<Product> products)
        {
            _uow.ProductRepository.RemoveRange(products);
            return await _uow.Save();
        }
        public async Task<bool> Update(Product product)
        {
            _uow.ProductRepository.Update(product);
            return await _uow.Save();
        }
    }
}
