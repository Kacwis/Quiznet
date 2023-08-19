using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using quiznet_api.Data;
using quiznet_api.Handlers.IHandlers;
using quiznet_api.Models;
using quiznet_api.Models.DTO;
using quiznet_api.Repository.IRepository;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace quiznet_api.Repository
{    
    public class UserRepository : IUserRepository
    {

        private readonly int JWT_TOKEN_EXPIRE_MINUTES = 60 * 24;

        private readonly ApplicationDbContext _db;

        private readonly IJwtHandler _jwtHandler;
                
        public UserRepository(ApplicationDbContext db, IJwtHandler jwtHandler)
        {
            _db = db;            
            _jwtHandler = jwtHandler;
        }
        
        public bool IsUniqueUser(string username)
        {
            var user = _db.Users.FirstOrDefault(x => x.Username == username);
            return user == null;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == loginRequestDTO.Username);
            if(user == null)
            {
                return null;
            }
            var salt = Convert.FromBase64String(user.Salt);            
            var encryptedPassword = EncryptPassword(loginRequestDTO.Password, salt);            
            if(encryptedPassword != user.Password)
            {
                return null;
            }            
             IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("user_id", user.Id.ToString()),                
            };
            var token = _jwtHandler.GenerateJwtToken(user.Username, claims, JWT_TOKEN_EXPIRE_MINUTES);
            var loginResonseDTO = new LoginResponseDTO()
            {                
                Token = token
            };
            return loginResonseDTO;                                
        }
        

        public async Task<User> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            var salt = GenerateSalt();
            User user = new()
            {
                Username = registrationRequestDTO.Username,
                Email = registrationRequestDTO.Email,
                Salt = Convert.ToBase64String(salt),
                Password = EncryptPassword(registrationRequestDTO.Password, salt),
                Role = registrationRequestDTO.Role
            };
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _db.Set<User>().Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task UpdatePassword(string password, User user)
        {
            var salt = GenerateSalt();
            var newPassword = EncryptPassword(password, salt);
            user.Salt = Convert.ToBase64String(salt);
            user.Password = newPassword;
            await UpdateAsync(user);
        }

        private string EncryptPassword(string password, byte[] salt)
        {

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltedPasswordBytes = new byte[passwordBytes.Length + salt.Length];
            Buffer.BlockCopy(passwordBytes, 0, saltedPasswordBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, saltedPasswordBytes, passwordBytes.Length, salt.Length);
            byte[] hashedPasswordBytes;
            using (var sha256 = SHA256.Create())
            {
                hashedPasswordBytes = sha256.ComputeHash(saltedPasswordBytes);
            }

            return Encoding.UTF8.GetString(hashedPasswordBytes);
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

    }


}
