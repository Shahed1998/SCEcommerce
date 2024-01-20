using entity.DataContext;
using Microsoft.EntityFrameworkCore;

namespace repository.Implementations
{
    public class Repository<TEntity> where TEntity : class
    {
        private readonly DatabaseContext _databaseContext;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _dbSet = databaseContext.Set<TEntity>();
        }

        public async void Delete(object Id)
        {
            var entity = await _dbSet.FindAsync(Id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public virtual IQueryable<TEntity> Get()
        {
            IQueryable<TEntity> query = _dbSet;
            return query;
        }

        public async virtual Task<TEntity?> GetById(int Id)
        {
            return await _dbSet.FindAsync(Id);
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.AddAsync(entity);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _databaseContext.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
