using entity.business_entities;
using entity.general_entities;
using manager.Interfaces;
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

        public IEnumerable<CategoryDTO> Get()
        {
            throw new NotImplementedException();
        }

        public CategoryDTO GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Insert(CategoryDTO dto)
        {
            dto.Id = 0;
            _unitOfWork.CategoryRepository.Insert((Category) dto);
            var ret = await _unitOfWork.Save();
            return ret;
        }

        public void Update(CategoryDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
