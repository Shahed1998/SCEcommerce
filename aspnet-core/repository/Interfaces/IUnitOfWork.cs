using entity.general_entities;
using repository.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repository.Interfaces
{
    public interface IUnitOfWork
    {
        Repository<Category> CategoryRepository { get; }
    }
}
