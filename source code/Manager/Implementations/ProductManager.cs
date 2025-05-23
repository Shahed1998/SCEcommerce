using AutoMapper;
using DataAccess.Repository.Interfaces;
using Manager.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ProductDTO> Get(int Id)
        {
            var product = (await _uow.ProductRepository.Get(x => x.Id == Id)) ?? new Product();
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> GetWithCategory(int Id)
        {
            var product = (await _uow.ProductRepository.GetAll()
                                                       .Where(x => x.Id == Id)
                                                       .Include(p => p.Category)
                                                       .FirstOrDefaultAsync()) ?? new Product();
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<PagedList> GetAll(int page, int pageSize)
        {
            try
            {
                var products = await _uow.ProductRepository.GetAll().Include(p => p.Category).OrderByDescending(x => x.Id)
                                                           .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                var totalCount = await _uow.ProductRepository.GetAll().CountAsync();

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
