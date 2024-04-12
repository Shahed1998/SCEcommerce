using entity.Business_Entities;
using entity.Business_Entities.User;
using manager.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {

            if(dto.UserName.IsNullOrEmpty() || dto.Password.IsNullOrEmpty())
            {
                return BadRequest(new { Status = "Failed", Message = "Username and Password is required" });
            }

            if(await _accountManager.UserAlreadyExist(dto.UserName!))
            {
                return BadRequest(new {Status = "Failed", Message = "Username already exist"});
            }

            bool result = await _accountManager.Register(dto);

            if(!result)
            {
                throw new Exception();
            }

            return Ok();
        }

    }
}
