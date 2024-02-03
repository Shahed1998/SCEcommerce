using entity.business_entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manager.Interfaces
{
    public interface ICategoryManager
    {
        Task<bool> Insert(CategoryDTO dto);
        IEnumerable<CategoryDTO> Get();
        CategoryDTO GetById(int Id);
        void Update(CategoryDTO dto);
        void Delete(object Id);
    }
}
