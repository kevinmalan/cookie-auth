using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Dtos.Requests;

namespace API.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IProfileService _profileService;
        private readonly TokenConfig _tokenConfig;

        public ProfileController(IProfileService profileService, IOptions<TokenConfig> tokenOptions)
        {
            _profileService = profileService;
            _tokenConfig = tokenOptions.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterProfileRequest request)
        {
            var tokens = await _profileService.CreateProfileAsync(request);
            var accessTokenOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.Add(_tokenConfig.AccessToken.Expires),
                HttpOnly = true,
                Domain = _tokenConfig.Issuer,
                Path = "/",
                Secure = true,
                IsEssential = true,
            };
            var idTokenOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.Add(_tokenConfig.IdToken.Expires),
                HttpOnly = false,
                Domain = _tokenConfig.Issuer, // TODO: Add domain for BE to match this. Then cookies should work.
                Path = "/",
                Secure = true,
                IsEssential = true,
            };
            var refreshTokenOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.Add(_tokenConfig.RefreshToken.Expires),
                HttpOnly = true,
                Domain = _tokenConfig.Issuer,
                Path = "/",
                Secure = true,
                IsEssential = true,
            };

            Response.Cookies.Append("cookie-auth-access-token", tokens.AccessToken, accessTokenOptions);
            Response.Cookies.Append("cookie-auth-refresh-token", tokens.RefreshToken, refreshTokenOptions);
            Response.Cookies.Append("cookie-auth-id-token", tokens.IdToken, idTokenOptions);

            return Ok();
        }
    }
}