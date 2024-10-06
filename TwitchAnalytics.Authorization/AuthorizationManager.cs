using System.IdentityModel.Tokens.Jwt;

namespace TwitchAnalytics.Authorization;

public class AuthorizationManager
{
    public bool IsTokenValid { get; set; }
    public Dictionary<string, string> Claims { get; } = [];
    public string? Issuer { get; }

    public AuthorizationManager(string? authorizationHeader)
    {
        try
        {
            if (authorizationHeader == null || authorizationHeader.StartsWith("Bearer "))
            {
                return;
            }
            string? token = authorizationHeader["Bearer ".Length..].Trim();

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
            var issuer = jwtToken.Issuer;

            IsTokenValid = true;
        }
        catch (Exception)
        {
            IsTokenValid = false;
        }
    }

    public void GenerateNewToken()
    {
        throw new NotImplementedException();
    }
}
