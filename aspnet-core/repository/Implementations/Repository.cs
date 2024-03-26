using entity.DataContext;
using entity.General_Entities;
using Microsoft.EntityFrameworkCore;
using repository.Interfaces;

namespace repository.Implementations
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DatabaseContext _databaseContext;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _dbSet = databaseContext.Set<TEntity>();
        }

        public virtual async Task Insert(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual IQueryable<TEntity> Get()
        {
            return _dbSet.AsQueryable();
        }

        public virtual async Task<TEntity?> GetById(int Id)
        {
            return await _dbSet.FindAsync(Id);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _databaseContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual async Task Delete(object Id)
        {
            var entity = await _dbSet.FindAsync(Id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public virtual async Task<User?> Authenticate(string username, string password)
        {
            if(typeof(TEntity) == typeof(User))
            {
                return await _dbSet
                              .Cast<User>()
                              .FirstOrDefaultAsync(x => x.UserName == username && x.Password == password);
            }

            return null;
        }
    }
}
