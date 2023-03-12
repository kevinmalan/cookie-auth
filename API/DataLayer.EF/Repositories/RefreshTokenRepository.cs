using DataLayer.EF.Contexts;
using DataLayer.EF.Entities;
using DataLayer.EF.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.EF.Repositories
{
    public class RefreshTokenRepository : BaseRepository, IRefreshTokenRepository
    {
        public RefreshTokenRepository(DataContext dataContext)
            : base(dataContext)
        {
        }

        public async Task<RefreshToken> GetLatestByProfileIdAsync(int profileId)
        {
            return await _dataContext.RefreshToken
                .AsNoTracking()
                .Where(x => x.ProfileId == profileId)
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync();
        }
    }
}