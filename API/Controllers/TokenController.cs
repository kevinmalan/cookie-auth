using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Exceptions;

namespace API.Controllers
{
    public class TokenController : BaseController
    {
        private readonly ITokenService _tokenService;
        private readonly TokenConfig _tokenConfig;

        public TokenController(ITokenService tokenService, IOptions<TokenConfig> tokenOptions)
        {
            _tokenConfig = tokenOptions.Value;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var accessToken = Request.Cookies["cookie-auth-access-token"];
            var idToken = Request.Cookies["cookie-auth-id-token"];
            var refreshToken = Request.Cookies["cookie-auth-refresh-token"];

            if (accessToken is null || idToken is null || refreshToken is null)
                throw new ForbiddenException("Not all the required auth cookies are present.");

            var tokens = await _tokenService.RefreshTokensAsync(accessToken, refreshToken);

            var accessTokenOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.Add(_tokenConfig.AccessToken.Expires),
                HttpOnly = true,
                Domain = _tokenConfig.Issuer,
                Path = "/",
                Secure = true,
                IsEssential = true,
            };
            var refreshTokenOptions = new CookieOptions
            {
                Expires = tokens.RefreshTokenExpiresOn,
                HttpOnly = true,
                Domain = _tokenConfig.Issuer,
                Path = "/",
                Secure = true,
                IsEssential = true,
            };

            Response.Cookies.Append("cookie-auth-access-token", tokens.AccessToken, accessTokenOptions);
            Response.Cookies.Append("cookie-auth-refresh-token", tokens.RefreshToken, refreshTokenOptions);

            return Ok();
        }
    }
}