using entity.Business_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manager.Interfaces
{
    public interface IAccountManager
    {
        Task<LoginResponseDTO?> Authenticate(LoginRequestDTO dto);
    }
}
