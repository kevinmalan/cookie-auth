using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Dtos.Requests;
using Shared.Dtos.Responses;

namespace API.Controllers
{
    public class ProfileController(IProfileService profileService, IOptions<TokenConfig> tokenOptions, IOptions<CookieConfig> cookieOptions) : BaseController
    {
        private readonly TokenConfig _tokenConfig = tokenOptions.Value;
        private readonly CookieConfig _cookieConfig = cookieOptions.Value;

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterProfileRequest request)
        {
            var tokens = await profileService.CreateAsync(request);
            IssueCookies(tokens);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginProfileRequest request)
        {
            var tokens = await profileService.LoginAsync(request);
            IssueCookies(tokens);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            foreach (var cookie in Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }

            return NoContent();
        }

        private void IssueCookies(AuthenticatedResponse tokens)
        {
            var accessTokenOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.Add(_cookieConfig.Expires),
                HttpOnly = true,
                Domain = _tokenConfig.Issuer,
                Path = "/",
                Secure = true,
                IsEssential = true,
            };
            var idTokenOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.Add(_cookieConfig.Expires),
                HttpOnly = false,
                Domain = _tokenConfig.Issuer,
                Path = "/",
                Secure = true,
                IsEssential = true,
            };
            var refreshTokenOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.Add(_cookieConfig.Expires),
                HttpOnly = true,
                Domain = _tokenConfig.Issuer,
                Path = "/",
                Secure = true,
                IsEssential = true,
            };

            Response.Cookies.Append("cookie-auth-access-token", tokens.AccessToken, accessTokenOptions);
            Response.Cookies.Append("cookie-auth-refresh-token", tokens.RefreshToken, refreshTokenOptions);
            Response.Cookies.Append("cookie-auth-id-token", tokens.IdToken, idTokenOptions);
        }
    }
}