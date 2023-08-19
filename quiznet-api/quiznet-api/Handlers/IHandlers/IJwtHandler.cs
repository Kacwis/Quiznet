using System.Security.Claims;

namespace quiznet_api.Handlers.IHandlers
{
    public interface IJwtHandler
    {
        int GetUserIdFromJwtToken(HttpContext httpContext);
       
        string GenerateJwtToken(string audience, IEnumerable<Claim> claims, int expiryMinute);

        bool IsTokenCloseToExpire(HttpContext httpContext);
    }
}
