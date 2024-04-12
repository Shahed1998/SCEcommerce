using entity.Business_Entities;
using entity.Business_Entities.User;
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
        Task<bool> Register(RegisterDTO dto);
        Task<bool> UserAlreadyExist(string username);
    }
}
