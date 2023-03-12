using DataLayer.EF.Contexts;
using DataLayer.EF.Entities;
using DataLayer.EF.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.EF.Repositories.Flows
{
    public class ProfileFlowRepository : BaseRepository, IProfileFlowRepository
    {
        public ProfileFlowRepository(DataContext dataContext)
            : base(dataContext)
        {
        }

        public async Task CreateProfileAndRefreshTokenAsync(Profile profile, RefreshToken refreshToken)
        {
            refreshToken.Profile = profile;
            await _dataContext.AddAsync(profile);
            await _dataContext.AddAsync(refreshToken);
            await _dataContext.SaveChangesAsync();
        }

        public async Task GuardAgainstExistingProfileAsync(string username)
        {
            var exists = await _dataContext.Profile.AnyAsync(x => x.Username == username);

            if (exists)
                throw new Exception(""); // Forbidden
        }

        public async Task GuardAgainstProfileNotExistingAsync(string username)
        {
            var exists = await _dataContext.Profile.AnyAsync(x => x.Username == username);

            if (!exists)
                throw new Exception(""); // Forbidden
        }

        public async Task<Profile> GetProfileByUsernameAsync(string username)
        {
            return await _dataContext.Profile.SingleAsync(x => x.Username == username);
        }
    }
}