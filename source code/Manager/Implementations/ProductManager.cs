using AutoMapper;
using DataAccess.Repository.Interfaces;
using Manager.Interfaces;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductManager(IUnitOfWork uow, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _uow = uow;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> Add(ProductVM product)
        {
            try
            {
                product.UniqueProductId = "p-" + Guid.NewGuid().ToString() + DateTime.Now.ToString("yyyyMMdd-HHmmss");
                await _uow.ProductRepository.Add(_mapper.Map<Product>(product));
                return await _uow.Save();
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }

        public async Task<ProductVM> Get(int Id)
        {
            var product = (await _uow.ProductRepository.Get(x => x.Id == Id)) ?? new Product();
            return _mapper.Map<ProductVM>(product);
        }

        public async Task<ProductVM> GetWithCategory(int Id)
        {
            var product = (await _uow.ProductRepository.GetAll()
                                                       .Where(x => x.Id == Id)
                                                       .Include(p => p.Category)
                                                       .FirstOrDefaultAsync()) ?? new Product();
            return _mapper.Map<ProductVM>(product);
        }

        public async Task<PagedList> GetAll(int page, int pageSize)
        {
            try
            {
                var products = await _uow.ProductRepository.GetAll().Include(p => p.Category).OrderByDescending(x => x.Id)
                                                           .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                var totalCount = await _uow.ProductRepository.GetAll().CountAsync();

                var result = new PagedList(page, pageSize, totalCount);

                result.products = _mapper.Map<IEnumerable<ProductVM>>(products);

                return result;
            }
            catch (Exception ex)
            {
                return new PagedList(page, pageSize, 0);
            }
        }

        public async Task<bool> Remove(int Id)
        {
            var productDto = await Get(Id);

            if (!string.IsNullOrEmpty(productDto.ImageUrl))
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                var imagePath = Path.Combine(wwwRootPath, productDto.ImageUrl.TrimStart('\\'));

                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            _uow.ProductRepository.Remove(_mapper.Map<Product>(productDto));
            return await _uow.Save();

            
        }

        public async Task<bool> RemoveRange(IEnumerable<Product> products)
        {
            _uow.ProductRepository.RemoveRange(products);
            return await _uow.Save();
        }
        public async Task<bool> Update(ProductVM product)
        {
            _uow.ProductRepository.Update(_mapper.Map<Product>(product));
            return await _uow.Save();
        }
    }
}
