using DataLayer.EF.Contexts;
using DataLayer.EF.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;

namespace DataLayer.EF.Repositories
{
    public class RefreshTokenRepository : BaseRepository, IRefreshTokenRepository
    {
        public RefreshTokenRepository(DataContext dataContext)
            : base(dataContext)
        {
        }

        public async Task GuardAgainstExpiring(string value)
        {
            var token = await DataContext.RefreshToken.FirstAsync(x => x.Value == value);
            if (token.ExpiresOn <= DateTimeOffset.UtcNow)
                throw new ForbiddenException("Refresh token has expired.");
        }

        public async Task<Domain.Models.RefreshToken> RenewAsync(string value, Guid profileLookupId, Guid oldAccessTokenId, Guid newAccessTokenId)
        {
            var refreshToken = await DataContext.RefreshToken.FirstOrDefaultAsync(x => x.Value == value);
            if (refreshToken is null || refreshToken.AccessTokenId != oldAccessTokenId)
                throw new ForbiddenException("Invalid refresh token");

            var newRefreshToken = new Domain.Models.RefreshToken
            {
                Value = $"{Guid.NewGuid()}",
                AccessTokenId = newAccessTokenId,
                ExpiresOn = refreshToken.ExpiresOn,
                LookupId = Guid.NewGuid()
            };

            var created = await CreateAsync(newRefreshToken, profileLookupId);

            return created;
        }

        public async Task<Domain.Models.RefreshToken> CreateAsync(Domain.Models.RefreshToken refreshTokenModel, Guid profileLookupId)
        {
            var refreshToken = refreshTokenModel.ToEntity();
            var profile = await DataContext.Profile.FirstAsync(x => x.LookupId == profileLookupId);     
            refreshToken.Profile = profile;
            await DataContext.AddAsync(refreshToken);
            await DataContext.SaveChangesAsync();

            return refreshToken.ToModel();
        }
    }
}