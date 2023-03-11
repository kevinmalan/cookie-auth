using Core.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Services
{
    public class TokenService : ITokenService
    {
        private readonly TokenConfig _tokenConfig;

        public TokenService(IOptions<TokenConfig> tokenOptions)
        {
            _tokenConfig = tokenOptions.Value;
        }

        public Tuple<JwtSecurityToken, string> CreateAccessToken(string username, string role)
        {
            var claims = new[]
            {
                new Claim("jti", $"{Guid.NewGuid()}"),
                new Claim("username", username),
                new Claim("role", role)
            };

            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.AccessToken.Secret));
            var token = new JwtSecurityToken(
                issuer: _tokenConfig.Issuer,
                audience: _tokenConfig.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(_tokenConfig.AccessToken.Expires),
                signingCredentials: new SigningCredentials(secret, SecurityAlgorithms.HmacSha256)
           );


            return Tuple.Create(token, new JwtSecurityTokenHandler().WriteToken(token));
        }

        public string CreateIdToken(string username)
        {
            var claims = new[]
            {
                new Claim("username", username)
            };

            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.IdToken.Secret));
            var token = new JwtSecurityToken(
                issuer: _tokenConfig.Issuer,
                audience: _tokenConfig.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(_tokenConfig.IdToken.Expires),
                signingCredentials: new SigningCredentials(secret, SecurityAlgorithms.HmacSha256)
           );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}