using DataAccess.Repository.Interfaces;
using Manager.Interfaces;
using Microsoft.Data.SqlClient;
using Models.Entities;
using Utility.Helpers;

namespace Manager.Implementations
{
    public class CategoryManager : ICategoryManager
    {

        private readonly IUnitOfWork _uow;

        public CategoryManager(IUnitOfWork uow)
        {
            _uow = uow;
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

        public async Task<PagedList> GetAll(int page, int pageSize)
        {

            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@PAGENUMBER", System.Data.SqlDbType.Int) { Value = page }, 
                new SqlParameter("@PAGESIZE", System.Data.SqlDbType.Int) { Value = pageSize }, 
            };

            string sql1 = "SELECT * FROM CATEGORY ORDER BY ID OFFSET @PAGESIZE*(@PAGENUMBER-1) ROWS" +
                " FETCH NEXT @PAGESIZE ROWS ONLY";

            var categories = _uow.CategoryRepository.ExecuteQuery(sql1, sqlParams);

            string sql2 = "SELECT COUNT(1) FROM CATEGORY";

            var totalCount = await _uow.CategoryRepository.ExecuteScalar<int>(sql2, sqlParams);

            var result = new PagedList(page, pageSize, totalCount);

            result.categories = categories;

            return result;

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
