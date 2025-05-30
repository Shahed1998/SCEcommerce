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

        public async Task<CategoryDTO> Get(int Id)
        {
            try
            {
                var category = await _uow.CategoryRepository.Get(x => x.Id == Id);

                if (category != null)
                {
                    return _mapper.Map<CategoryDTO>(category);
                }
            }
            catch (Exception ex) 
            {
                HelperSerilog.LogError(ex.Message, ex);
            }

            return new CategoryDTO();
        }

        public async Task<PagedList> GetAll(int page, int pageSize, bool getAll = false)
        {

            try
            {

                var categories = await _uow.CategoryRepository.GetAll().OrderByDescending(x => x.Id)
                                                              .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                var totalCount = await _uow.CategoryRepository.GetAll().CountAsync();

                var result = new PagedList(page, pageSize, totalCount);

                result.categories = _mapper.Map<IEnumerable<CategoryDTO>>(categories);

                return result;
            }
            catch (Exception ex)
            {
                return new PagedList(page, pageSize, 0);
            }

        }

        public async Task<bool> Remove(CategoryDTO entity)
        {
            _uow.CategoryRepository.Remove(_mapper.Map<Category>(entity));
            return await _uow.Save();
        }

        public async Task<bool> RemoveRange(IEnumerable<Category> entities)
        {
            _uow.CategoryRepository.RemoveRange(entities);
            return await _uow.Save();
        }

        public async Task<bool> Update(CategoryDTO category)
        {
            _uow.CategoryRepository.Update(_mapper.Map<Category>(category));
            return await _uow.Save();
        }
    }
}
