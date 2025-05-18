using AutoMapper;
using DataAccess.Repository.Interfaces;
using Manager.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models.BusinessEntities;
using Models.Entities;
using Utility.Helpers;

namespace Manager.Implementations
{
    public class CategoryManager : ICategoryManager
    {

        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CategoryManager(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<CategoryDTO>> All()
        {
            var categoryList = await _uow.CategoryRepository.GetAll().ToListAsync();
            return _mapper.Map<List<CategoryDTO>>(categoryList);
        }

        public async Task<bool> Add(Category entity)
        {
            await _uow.CategoryRepository.Add(entity);
            return await _uow.Save();
        }

        public async Task<Category> Get(int Id)
        {
            return (await _uow.CategoryRepository.Get(x => x.Id == Id)) ?? new Category();
        }

        public async Task<PagedList> GetAll(int page, int pageSize, bool getAll = false)
        {

            try
            {
                var sqlParams = new List<SqlParameter>
                {
                    new SqlParameter("@PAGENUMBER", System.Data.SqlDbType.Int) { Value = page },
                    new SqlParameter("@PAGESIZE", System.Data.SqlDbType.Int) { Value = pageSize },
                    //new SqlParameter("@GETALL", System.Data.SqlDbType.Bit) { Value = getAll },
                };

                var sql = "EXEC usp_GetAllCategories @PAGENUMBER=@PAGENUMBER, @PAGESIZE=@PAGESIZE";

                var categories = await _uow.CategoryRepository.ExecuteQuery(sql, sqlParams.ToArray());

                sql += " , @TOTALCOUNT=@TOTALCOUNT";

                sqlParams.Add(new SqlParameter("@TOTALCOUNT", System.Data.SqlDbType.Bit) { Value = true });

                var totalCount = await _uow.CategoryRepository.ExecuteScalar<int>(sql, sqlParams.ToArray());

                var result = new PagedList(page, pageSize, totalCount);

                result.categories = _mapper.Map<IEnumerable<CategoryDTO>>(categories);

                return result;
            }
            catch (Exception ex)
            {
                return new PagedList(page, pageSize, 0);
            }

        }

        public async Task<bool> Remove(Category entity)
        {
            _uow.CategoryRepository.Remove(entity);
            return await _uow.Save();
        }

        public async Task<bool> RemoveRange(IEnumerable<Category> entities)
        {
            _uow.CategoryRepository.RemoveRange(entities);
            return await _uow.Save();
        }

        public async Task<bool> Update(Category category)
        {
            _uow.CategoryRepository.Update(category);
            return await _uow.Save();
        }
    }
}
