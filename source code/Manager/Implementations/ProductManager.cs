using AutoMapper;
using DataAccess.Repository.Interfaces;
using Manager.Interfaces;
using Microsoft.Data.SqlClient;
using Models.BusinessEntities;
using Models.Entities;
using Utility.Helpers;

namespace Manager.Implementations
{
    public class ProductManager : IProductManager
    {

        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ProductManager(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> Add(ProductDTO product)
        {
            try
            {
                await _uow.ProductRepository.Add(_mapper.Map<Product>(product));
                return await _uow.Save();
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }

        public async Task<Product> Get(int Id)
        {
            return (await _uow.ProductRepository.Get(x => x.Id == Id)) ?? new Product();
        }

        public async Task<PagedList> GetAll(int page, int pageSize)
        {
            try
            {
                var sqlParams = new List<SqlParameter>
                {
                   new SqlParameter("@PAGENUMBER", System.Data.SqlDbType.Int) { Value = page },
                   new SqlParameter("@PAGESIZE", System.Data.SqlDbType.Int) { Value = pageSize },
                };

                var sql = "EXEC usp_GetAllProducts @PAGENUMBER=@PAGENUMBER, @PAGESIZE=@PAGESIZE";

                var products = await _uow.ProductRepository.ExecuteQuery(sql, sqlParams.ToArray());

                sql += " , @TOTALCOUNT=@TOTALCOUNT";

                sqlParams.Add(new SqlParameter("@TOTALCOUNT", System.Data.SqlDbType.Bit) { Value = true });

                var totalCount = await _uow.ProductRepository.ExecuteScalar<int>(sql, sqlParams.ToArray());

                var result = new PagedList(page, pageSize, totalCount);

                result.products = _mapper.Map<IEnumerable<ProductDTO>>(products);

                return result;
            }
            catch (Exception ex)
            {
                return new PagedList(page, pageSize, 0);
            }
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
