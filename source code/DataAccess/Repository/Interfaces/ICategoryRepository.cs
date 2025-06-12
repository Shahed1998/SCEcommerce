using Models.Entities;

namespace DataAccess.Repository.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
        bool DisplayOrderAlreadyExist(int displayOrder, int? Id);
    }
}
