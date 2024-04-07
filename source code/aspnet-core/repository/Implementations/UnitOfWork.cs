using entity.DataContext;
using entity.general_entities;
using entity.General_Entities;
using repository.Interfaces;

namespace repository.Implementations
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<User> _userRepository;

        public UnitOfWork(
            DatabaseContext databaseContext,
            IRepository<Category> categoryRepository,
            IRepository<User> userRepository
        ) 
        {
            _databaseContext = databaseContext;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        #region Repositories
        IRepository<Category> IUnitOfWork.CategoryRepository
        {
            get
            { 
                return _categoryRepository ?? new Repository<Category>(_databaseContext);
            }
        }
        IRepository<User> IUnitOfWork.UserRepository
        {
            get
            {
                return _userRepository ?? new Repository<User>(_databaseContext);
            }
        }
        #endregion

        #region Transactions
        public async Task<bool> Save()
        {
            return await _databaseContext.SaveChangesAsync() > 0;
        }
        #endregion

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
