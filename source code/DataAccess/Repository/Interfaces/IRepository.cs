using Microsoft.Data.SqlClient;
using System.Linq.Expressions;

namespace DataAccess.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T?> Get(Expression<Func<T, bool>> predicate);
        Task Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> ExecuteQuery(string sql, SqlParameter[] parameters);
        Task<TResult> ExecuteScalar<TResult>(string sql, SqlParameter[] parameters);
    }
}
