using Microsoft.IdentityModel.Tokens;
using quiznet_api.Handlers.IHandlers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace quiznet_api.Handlers
{
    public class JwtHandler : IJwtHandler
    {
        private static readonly string ISSUER = "Quiznet";

        private static readonly int TIME_LEFT_TO_EXPIRE = 2;

        public int GetUserIdFromJwtToken(HttpContext httpContext)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var userId = int.Parse(jwtToken.Claims.FirstOrDefault(c => c.Type == "user_id").Value);
            return userId;
        }

        public bool IsTokenCloseToExpire(HttpContext httpContext) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var jwtToken = tokenHandler.ReadJwtToken(token);
            return jwtToken.ValidTo.Subtract(TimeSpan.FromMinutes(TIME_LEFT_TO_EXPIRE)) <= DateTime.UtcNow;            
        }

        public string GenerateJwtToken(string audience, IEnumerable<Claim> claims, int expiryMinutes)
        {
            var secretKey = System.Environment.GetEnvironmentVariable("SECRET_KEY");
            
            System.Diagnostics.Debug.Write(secretKey);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: ISSUER,
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
