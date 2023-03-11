using Core.Contexts;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Dtos.Requests;
using Shared.Dtos.Responses;

namespace Core.Services
{
    public class ProfileService : IProfileService
    {
        private readonly DataContext _dataContext;
        private readonly ICryptographicService _cryptographicService;
        private readonly ITokenService _tokenService;
        private readonly TokenConfig _tokenConfig;

        public ProfileService(DataContext dataContext, ICryptographicService cryptographicService, ITokenService tokenService, IOptions<TokenConfig> tokenOptions)
        {
            _dataContext = dataContext;
            _cryptographicService = cryptographicService;
            _tokenService = tokenService;
            _tokenConfig = tokenOptions.Value;
        }

        public async Task<AuthenticatedResponse> CreateProfileAsync(RegisterProfileRequest request)
        {
            await GuardAgainstNullOrWhiteSpace(request.Username, request.Password);
            await GuardAgainstExistingProfileAsync(request.Username);
            ValidatePasswordStrength(request.Password);
            var role = "Admin";
            var password = _cryptographicService.HashPassword(request.Password);
            var accessTokenTuple = _tokenService.CreateAccessToken(request.Username, role);
            var idToken = _tokenService.CreateIdToken(request.Username);

            var profile = new DomainModels.Profile
            {
                Username = request.Username,
                LookupId = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow,
                PasswordHash = password.Hash,
                Salt = password.Salt,
                Role = role,
            };
            var refreshToken = new DomainModels.RefreshToken
            {
                AccessTokenId = Guid.Parse(accessTokenTuple.Item1.Id),
                ExpiresOn = DateTimeOffset.UtcNow.Add(_tokenConfig.RefreshToken.Expires),
                LookupId = Guid.NewGuid(),
                Value = $"{Guid.NewGuid()}",
                Profile = profile,
            };

            await _dataContext.AddAsync(profile);
            await _dataContext.AddAsync(refreshToken);
            await _dataContext.SaveChangesAsync();

            return new AuthenticatedResponse
            {
                AccessToken = accessTokenTuple.Item2,
                IdToken = idToken,
                RefreshToken = refreshToken.Value
            };
        }

        private async Task GuardAgainstNullOrWhiteSpace(params string[] values)
        {
            foreach (var item in values)
            {
                if (string.IsNullOrWhiteSpace(item))
                    throw new Exception(); // BadRequest
            }
        }

        private async Task GuardAgainstExistingProfileAsync(string username)
        {
            var exists = await _dataContext.Profile.AnyAsync(x => x.Username == username);

            if (exists)
                throw new Exception(""); // Forbidden
        }

        private static void ValidatePasswordStrength(string password)
        {
            if (password.Length < 9 || !password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit) || !password.Any(c => !char.IsLetterOrDigit(c)))
            {
                throw new Exception("Password strength failed"); // Badrequest
            }
        }
    }
}