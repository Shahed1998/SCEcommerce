using entity.general_entities;

namespace repository.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Category> CategoryRepository { get; }
        Task<bool> Save();
        void Dispose();
    }
}
