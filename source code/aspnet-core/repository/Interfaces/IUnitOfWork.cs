using entity.general_entities;
using entity.General_Entities;

namespace repository.Interfaces
{
    public interface IUnitOfWork
    {
        #region Repositories
        IRepository<Category> CategoryRepository { get; }
        IRepository<User> UserRepository { get; }
        IRepository<Role> RoleRepository {  get; }
        #endregion

        Task<bool> Save();
        void Dispose();
    }
}
