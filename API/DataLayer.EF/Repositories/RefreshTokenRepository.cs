using DataLayer.EF.Contexts;
using DataLayer.EF.Interfaces;

namespace DataLayer.EF.Repositories
{
    public class RefreshTokenRepository : BaseRepository, IRefreshTokenRepository
    {
        public RefreshTokenRepository(DataContext dataContext)
            : base(dataContext)
        {
        }

        public async Task<Domain.Models.RefreshToken> CreateIfNotExistsAsync(Domain.Models.RefreshToken refreshTokenModel, Guid profileLookupId)
        {
            var query = from p in DataContext.Profile
                        join r in DataContext.RefreshToken
                        on p.Id equals r.Id into profileLeftJoin
                        from token in profileLeftJoin.DefaultIfEmpty()
                        where p.LookupId == profileLookupId
                        orderby token.ExpiresOn descending
                        select new 
                        {
                            refreshToken = token,
                            profile = p
                        };

            var result = query.FirstOrDefault();
            var refreshToken = result?.refreshToken;
            var profile = result?.profile ?? throw new Exception($"No profile found for lookup value '{profileLookupId}'");
            if (refreshToken is null || refreshToken.ExpiresOn <= DateTimeOffset.UtcNow)
            {
                refreshToken = refreshTokenModel.ToEntity();
                refreshToken.Profile = profile;
                await DataContext.AddAsync(refreshToken);
                await DataContext.SaveChangesAsync();
            }

            return refreshToken.ToModel();
        }
    }
}