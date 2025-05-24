using DataAccess.Data;
using DataAccess.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace DataAccess.Repository.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _db = context;
            this._dbSet = _db.Set<T>();
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }


        public async Task<T?> Get(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking();
        }

        public void Remove(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
            }
            catch(Exception ex)
            {

            }
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<IEnumerable<T>?> ExecuteQuery(string sql, SqlParameter[] parameters) 
        {
            try
            {
                return await _dbSet.FromSqlRaw(sql, parameters).ToListAsync();
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<TResult> ExecuteScalar<TResult>(string sql, SqlParameter[] parameters)
        {
            try
            {
                using var command = _db.Database.GetDbConnection().CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                if (parameters != null && parameters.Length > 0)
                    command.Parameters.AddRange(parameters);

                var wasClosed = command.Connection!.State == ConnectionState.Closed;
                if (wasClosed)
                    command.Connection.Open();

                var result = await command.ExecuteScalarAsync();

                if (wasClosed)
                    command.Connection.Close();

                return result == DBNull.Value || result == null
                    ? default!
                    : (TResult)Convert.ChangeType(result, typeof(TResult));
            }
            catch(Exception ex)
            {
                return default;

            }
        }

    }
}
