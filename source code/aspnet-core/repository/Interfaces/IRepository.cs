using entity.General_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repository.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task Insert(TEntity entity);
        IQueryable<TEntity> Get();
        Task<TEntity?> GetById(int Id);
        void Update(TEntity entityToUpdate);
        Task Delete(object Id);
        Task<User?> Authenticate(string username, string password);
    }
}
