using entity.Business_Entities;
using manager.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountManager _accountManager;
        public AccountController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDTO dto)
        {
            LoginResponseDTO? response = await _accountManager.Authenticate(dto);

            if (response == null) 
            {
                return Unauthorized();
            }

            return Ok(response);
        }
    }
}
