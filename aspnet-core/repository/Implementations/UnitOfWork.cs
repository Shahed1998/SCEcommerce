using entity.DataContext;
using entity.general_entities;
using repository.Interfaces;

namespace repository.Implementations
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DatabaseContext _databaseContext;
        private readonly Repository<Category> _categoryRepository;
        public UnitOfWork(
            DatabaseContext databaseContext,
            Repository<Category> categoryRepository
        ) 
        {
            _databaseContext = databaseContext;
            _categoryRepository = categoryRepository;
        }

        public Repository<Category> CategoryRepository 
        {
            get
            {
                return _categoryRepository == null ? new Repository<Category>(_databaseContext) : _categoryRepository;
            }
        }

        public bool Save()
        {
            return _databaseContext.SaveChanges() > 0;
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
