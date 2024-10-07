using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Dtos.Responses;
using Shared.Exceptions;

namespace Domain.Services
{
    public class CookieService(
        IOptions<TokenConfig> tokenOptions,
        IOptions<CookieConfig> cookieOptions,
        IHttpContextAccessor httpContextAccessor,
        ITokenService tokenService) : ICookieService
    {
        public void IssueAuthCookies(AuthenticatedResponse tokens)
        {
            var accessTokenOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.Add(cookieOptions.Value.Expires),
                HttpOnly = true,
                Domain = tokenOptions.Value.Issuer,
                Path = "/",
                Secure = true,
                IsEssential = true,
            };
            var idTokenOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.Add(cookieOptions.Value.Expires),
                HttpOnly = false,
                Domain = tokenOptions.Value.Issuer,
                Path = "/",
                Secure = true,
                IsEssential = true,
            };
            var refreshTokenOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.Add(cookieOptions.Value.Expires),
                HttpOnly = true,
                Domain = tokenOptions.Value.Issuer,
                Path = "/",
                Secure = true,
                IsEssential = true,
            };

            httpContextAccessor.HttpContext.Response.Cookies.Append("cookie-auth-access-token", tokens.AccessToken, accessTokenOptions);
            httpContextAccessor.HttpContext.Response.Cookies.Append("cookie-auth-refresh-token", tokens.RefreshToken, refreshTokenOptions);
            httpContextAccessor.HttpContext.Response.Cookies.Append("cookie-auth-id-token", tokens.IdToken, idTokenOptions);
        }

        public async Task RefreshAuthCookiesAsync()
        {
            var accessToken = httpContextAccessor.HttpContext.Request.Cookies["cookie-auth-access-token"];
            var idToken = httpContextAccessor.HttpContext.Request.Cookies["cookie-auth-id-token"];
            var refreshToken = httpContextAccessor.HttpContext.Request.Cookies["cookie-auth-refresh-token"];

            if (accessToken is null || idToken is null || refreshToken is null)
                throw new ForbiddenException("Not all the required auth cookies are present.");

            var tokens = await tokenService.RefreshTokensAsync(accessToken, refreshToken);

            var accessTokenOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.Add(tokenOptions.Value.AccessToken.Expires),
                HttpOnly = true,
                Domain = tokenOptions.Value.Issuer,
                Path = "/",
                Secure = true,
                IsEssential = true,
            };
            var refreshTokenOptions = new CookieOptions
            {
                Expires = tokens.RefreshTokenExpiresOn,
                HttpOnly = true,
                Domain = tokenOptions.Value.Issuer,
                Path = "/",
                Secure = true,
                IsEssential = true,
            };

            httpContextAccessor.HttpContext.Response.Cookies.Append("cookie-auth-access-token", tokens.AccessToken, accessTokenOptions);
            httpContextAccessor.HttpContext.Response.Cookies.Append("cookie-auth-refresh-token", tokens.RefreshToken, refreshTokenOptions);
        }

        public void DeleteAuthCookies()
        {
            foreach (var cookie in httpContextAccessor.HttpContext.Request.Cookies)
                httpContextAccessor.HttpContext.Response.Cookies.Delete(cookie.Key);
        }
    }
}