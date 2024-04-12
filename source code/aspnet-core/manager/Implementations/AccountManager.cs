using entity.Business_Entities;
using entity.Business_Entities.User;
using entity.General_Entities;
using manager.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
            User? result = await _unitOfWork.UserRepository.Authenticate(dto.UserName);

            if(result == null || !MatchedPasswordHash(dto.Password, result.Password!, result.PasswordKey!))
            {
                return null;
            }

            return new LoginResponseDTO() { UserName = result.UserName, Token = GetJWTToken(result) };
        }

        public async Task<bool> Register(RegisterDTO dto)
        {
            byte[] passwordHash, passwordKey;

            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password!));
            }

            int customerRoleId = 2;

            int.TryParse(_configuration.GetSection("Settings:CustomerRoleId").Value, out customerRoleId);

            User user = new User();

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.UserName = dto.UserName;
            user.Password = passwordHash;
            user.PasswordKey = passwordKey;
            user.Email = dto.Email;
            user.Role = await _unitOfWork.RoleRepository.GetById(customerRoleId);

            await _unitOfWork.UserRepository.Insert(user);
            return await _unitOfWork.Save();

        }

        public Task<bool> UserAlreadyExist(string username)
        {
            return _unitOfWork.UserRepository.Any(x => x.UserName == username);
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

        private bool MatchedPasswordHash(string password, byte[] passwordHash, byte[] passwordKey)
        {
            using(var hmac = new HMACSHA512(passwordKey))
            {
                var hashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                return hashedPassword.SequenceEqual(passwordHash);
            }
        }
    }
}
