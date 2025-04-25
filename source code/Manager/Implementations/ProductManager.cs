using DataAccess.Repository.Interfaces;
using Manager.Interfaces;
using Models.Entities;

namespace Manager.Implementations
{
    public class ProductManager : IProductManager
    {

        private readonly IUnitOfWork _uow;

        public ProductManager(IUnitOfWork uow)
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

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _uow.CategoryRepository.GetAll();
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
