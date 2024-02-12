using entity.business_entities;
using entity.general_entities;
using manager.Interfaces;
using Microsoft.EntityFrameworkCore;
using repository.Interfaces;

namespace manager.Implementations
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryManager(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public void Delete(object Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CategoryDTO>> Get()
        {
            var categories =  await _unitOfWork.CategoryRepository.Get().AsNoTracking().ToListAsync();
            return categories.Select(category => (CategoryDTO) category);
        }

        public async Task<CategoryDTO?> GetById(int Id)
        {
            var category = await _unitOfWork.CategoryRepository.GetById(Id);
            return category == null ? null : (CategoryDTO) category;
        }

        public async Task<bool> Insert(CategoryDTO dto)
        {
            dto.Id = 0;
            await _unitOfWork.CategoryRepository.Insert((Category) dto);
            var ret = await _unitOfWork.Save();
            return ret;
        }

        public void Update(CategoryDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
