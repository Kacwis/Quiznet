using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using quiznet_api.Data;
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
        private readonly ApplicationDbContext _db;

        private readonly IConfiguration _configuration;

        private readonly IMapper _mapper;
        
        public UserRepository(ApplicationDbContext db, IConfiguration configuration, IMapper mapper)
        {
            _db = db;
            _configuration = configuration;
            _mapper = mapper;           
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
            Debug.WriteLine(user.Salt, salt.ToString());
            var encryptedPassword = EncryptPassword(loginRequestDTO.Password, salt);
            Debug.WriteLine(encryptedPassword, user.Password);
            if(encryptedPassword != user.Password)
            {
                return null;
            }
            
             IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("user_id", user.Id.ToString()),                
            };
            var token = GenerateJwtToken("Quiznet", user.Username, claims, 20);
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
                Role = "ROLE_USER"
            };
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }

        private static string EncryptPassword(string password, byte[] salt)
        {
            
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltedPasswordBytes = new byte[passwordBytes.Length + salt.Length];
            Buffer.BlockCopy(passwordBytes, 0, saltedPasswordBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, saltedPasswordBytes, passwordBytes.Length, salt.Length);
            byte[] hashedPasswordBytes;
            using(var sha256 = SHA256.Create())
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

        private string GenerateJwtToken(string issuer, string audience, IEnumerable<Claim> claims, int expiryMinutes)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("ApiSettings:SecretKey")));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: signingCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }

    }


}
