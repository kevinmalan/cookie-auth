﻿using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Dtos.Requests;
using Shared.Dtos.Responses;
using DataLayer.EF.Interfaces;
using Shared.Exceptions;

namespace Domain.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ICryptographicService _cryptographicService;
        private readonly ITokenService _tokenService;
        private readonly IProfileFlowRepository _profileFlowRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly TokenConfig _tokenConfig;

        public ProfileService(
            ICryptographicService cryptographicService, 
            ITokenService tokenService, 
            IOptions<TokenConfig> tokenOptions, 
            IProfileFlowRepository profileFlowRepository, 
            IRefreshTokenRepository refreshTokenRepository)
        {
            _cryptographicService = cryptographicService;
            _tokenService = tokenService;
            _profileFlowRepository = profileFlowRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenConfig = tokenOptions.Value;
        }

        public async Task<AuthenticatedResponse> CreateAsync(RegisterProfileRequest request)
        {
            GuardAgainstNullOrWhiteSpace(request.Username, request.Password);
            await _profileFlowRepository.GuardAgainstExistingProfileAsync(request.Username);
            ValidatePasswordStrength(request.Password);
            var role = "Admin";
            var profileLookupId = Guid.NewGuid();
            var password = _cryptographicService.HashPassword(request.Password);
            var accessTokenTuple = _tokenService.CreateAccessToken(request.Username, role, $"{profileLookupId}");
            var idToken = _tokenService.CreateIdToken(request.Username);

            var profile = new Models.Profile
            {
                Username = request.Username,
                LookupId = profileLookupId,
                CreatedOn = DateTime.UtcNow,
                PasswordHash = password.Hash,
                Salt = password.Salt,
                Role = role,
            };
            var refreshToken = new Models.RefreshToken
            {
                AccessTokenId = Guid.Parse(accessTokenTuple.Item1.Id),
                ExpiresOn = DateTimeOffset.UtcNow.Add(_tokenConfig.RefreshToken.Expires),
                LookupId = Guid.NewGuid(),
                Value = $"{Guid.NewGuid()}"
            };

            await _profileFlowRepository.CreateProfileAndRefreshTokenAsync(profile, refreshToken);

            return new AuthenticatedResponse
            {
                AccessToken = accessTokenTuple.Item2,
                IdToken = idToken,
                RefreshToken = refreshToken.Value
            };
        }

        public async Task<AuthenticatedResponse> LoginAsync(LoginProfileRequest request)
        {
            GuardAgainstNullOrWhiteSpace(request.Username, request.Password);
            await _profileFlowRepository.GuardAgainstProfileNotExistingAsync(request.Username);
            var profile = await _profileFlowRepository.GetProfileByUsernameAsync(request.Username);
            var hashed = _cryptographicService.HashPassword(request.Password, profile.Salt);
            if (profile.PasswordHash != hashed.Hash)
                throw new ForbiddenException("Incorrect username / password.");
            var role = "Admin";
            var accessTokenTuple = _tokenService.CreateAccessToken(request.Username, role, $"{profile.LookupId}");
            var idToken = _tokenService.CreateIdToken(request.Username);
            var refreshToken = await _tokenService.CreateRefreshTokenAsync(accessTokenTuple.Item1.Id, profile.LookupId);
            return new AuthenticatedResponse
            {
                AccessToken = accessTokenTuple.Item2,
                IdToken = idToken,
                RefreshToken = refreshToken.Value
            };
        }

        private static void GuardAgainstNullOrWhiteSpace(params string[] values)
        {
            foreach (var item in values)
            {
                if (string.IsNullOrWhiteSpace(item))
                    throw new BadRequestException("Not all required fields were set");
            }
        }

        private static void ValidatePasswordStrength(string password)
        {
            if (password.Length < 9 || !password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit) || !password.Any(c => !char.IsLetterOrDigit(c)))
            {
                throw new BadRequestException("Password strength failed");
            }
        }
    }
}