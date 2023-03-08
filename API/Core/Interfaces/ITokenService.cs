using System.IdentityModel.Tokens.Jwt;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        Tuple<JwtSecurityToken, string> CreateAccessToken(string username, string role);
        string CreateIdToken(string username);
    }
}