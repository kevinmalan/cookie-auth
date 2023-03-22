using DataLayer.EF.Interfaces;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Configuration;
using Shared.Dtos.Responses;
using Shared.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Domain.Services
{
    public class TokenService : ITokenService
    {
        private readonly TokenConfig _tokenConfig;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public TokenService(IOptions<TokenConfig> tokenOptions, IRefreshTokenRepository refreshTokenRepository)
        {
            _tokenConfig = tokenOptions.Value;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public Tuple<JwtSecurityToken, string> CreateAccessToken(string username, string role, string profileLookupId)
        {
            var claims = new[]
            {
                new Claim("jti", $"{Guid.NewGuid()}"),
                new Claim("username", username),
                new Claim("role", role),
                new Claim("profile-lookup-id", profileLookupId)
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

        public async Task<Models.RefreshToken> CreateRefreshTokenAsync(string accessTokenId, Guid profileLookupId, Guid? lookupId = null)
        {
            var refreshToken = new Models.RefreshToken
            {
                AccessTokenId = Guid.Parse(accessTokenId),
                ExpiresOn = DateTimeOffset.UtcNow.Add(_tokenConfig.RefreshToken.Expires),
                LookupId = lookupId ?? Guid.NewGuid(),
                Value = $"{Guid.NewGuid()}"
            };

            return await _refreshTokenRepository.CreateAsync(refreshToken, profileLookupId);
        }

        public async Task<RefreshAuthenticatedResponse> RefreshTokensAsync(string accessToken, string refreshToken)
        {
            var oldAccessToken = ValidateAccessToken(accessToken);
            if (oldAccessToken.ValidTo > DateTime.UtcNow)
                throw new BadRequestException("Access token has not expired yet");

            await _refreshTokenRepository.GuardAgainstExpiring(refreshToken);

            var username = oldAccessToken.Claims.First(x => x.Type == "username").Value;
            var role = oldAccessToken.Claims.First(x => x.Type == "role").Value;
            var profileLookupId = oldAccessToken.Claims.First(x => x.Type == "profile-lookup-id").Value;
            var newAccessToken = CreateAccessToken(username, role, profileLookupId);
            var newRefreshToken = await _refreshTokenRepository.RenewAsync(refreshToken, Guid.Parse(profileLookupId), Guid.Parse(oldAccessToken.Id), Guid.Parse(newAccessToken.Item1.Id));

            return new RefreshAuthenticatedResponse
            {
                AccessToken = newAccessToken.Item2,
                RefreshToken = newRefreshToken.Value,
                RefreshTokenExpiresOn = newRefreshToken.ExpiresOn
            };
        }

        private JwtSecurityToken ValidateAccessToken(string accessToken)
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.AccessToken.Secret));
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = secret,
                ValidIssuer = _tokenConfig.Issuer,
                ValidAudience = _tokenConfig.Audience,
                ValidateIssuer = true,
                ValidateAudience = true,
            };
            SecurityToken? token;
            try
            {
                handler.ValidateToken(accessToken, validationParameters, out token);
            }
            catch (Exception)
            {
                throw new ForbiddenException("invalid access token");
            }

            return (JwtSecurityToken)token;
        }
    }
}