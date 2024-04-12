using entity.General_Entities;
using System.Linq.Expressions;

namespace repository.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task Insert(TEntity entity);
        IQueryable<TEntity> Get();
        Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> GetById(int Id);
        void Update(TEntity entityToUpdate);
        Task Delete(object Id);
        Task<User?> Authenticate(string username);
    }
}
