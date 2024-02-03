using entity.DataContext;
using entity.general_entities;
using repository.Interfaces;

namespace repository.Implementations
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IRepository<Category> _categoryRepository;
        public UnitOfWork(
            DatabaseContext databaseContext,
            IRepository<Category> categoryRepository
        ) 
        {
            _databaseContext = databaseContext;
            _categoryRepository = categoryRepository;
        }

        #region Repositories
        IRepository<Category> IUnitOfWork.CategoryRepository
        {
            get
            { 
                return _categoryRepository ?? new Repository<Category>(_databaseContext);
            }
        }
        #endregion

        public async Task<bool> Save()
        {
            return await _databaseContext.SaveChangesAsync() > 0;
        }

        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _databaseContext.Dispose();
                }
            }
            this.disposed = true;
        }

        // called explicitly by the programmer
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
