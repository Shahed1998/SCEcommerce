using entity.Business_Entities;
using entity.General_Entities;
using manager.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace manager.Implementations
{
    public class AccountManager : IAccountManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public AccountManager(IUnitOfWork unitOfWork, IConfiguration configuration) 
        { 
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<LoginResponseDTO?> Authenticate(LoginRequestDTO dto)
        {
            User? result = await _unitOfWork.UserRepository.Authenticate(dto.UserName, dto.Password);

            if(result == null)
            {
                return null;
            }

            return new LoginResponseDTO() { UserName = result.UserName, Token = GetJWTToken(result) };
        }

        private string GetJWTToken(User user)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configuration.GetSection("Settings:JWTSecretKey").Value!));

                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                };

                var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                int jWTExpiryMinutes = 60;
                string? JWTExpiryMinutes = _configuration.GetSection("Settings:JWTExpiryMinutes").Value;
                int.TryParse(JWTExpiryMinutes, out jWTExpiryMinutes);

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(jWTExpiryMinutes),
                    SigningCredentials = signingCredentials,
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            
        }
    }
}
