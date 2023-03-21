using Shared.Dtos.Responses;
using System.IdentityModel.Tokens.Jwt;

namespace Domain.Interfaces
{
    public interface ITokenService
    {
        Tuple<JwtSecurityToken, string> CreateAccessToken(string username, string role, string profileLookupId);
        string CreateIdToken(string username);
        Task<Models.RefreshToken> CreateRefreshTokenAsync(string accessTokenId, Guid profileLookupId, Guid? lookupId = null);

        Task<RefreshAuthenticatedResponse> RefreshTokensAsync(string accessToken, string refreshToken);
    }
}