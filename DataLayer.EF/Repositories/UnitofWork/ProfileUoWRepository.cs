using DataLayer.EF.Contexts;
using DataLayer.EF.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;

namespace DataLayer.EF.Repositories.UnitOfWork
{
    public class ProfileUoWRepository : BaseRepository, IProfileFlowRepository
    {
        public ProfileUoWRepository(DataContext dataContext)
            : base(dataContext)
        {
        }

        public async Task CreateProfileAndRefreshTokenAsync(Domain.Models.Profile profileModel, Domain.Models.RefreshToken refreshTokenModel)
        {
            var profile = profileModel.ToEntity();
            var refreshToken = refreshTokenModel.ToEntity();
            refreshToken.Profile = profile;
            await DataContext.AddAsync(profile);
            await DataContext.AddAsync(refreshToken);
            await DataContext.SaveChangesAsync();
        }

        public async Task GuardAgainstExistingProfileAsync(string username)
        {
            var exists = await DataContext.Profile.AnyAsync(x => x.Username == username);

            if (exists)
                throw new ForbiddenException("Username already taken.");
        }

        public async Task GuardAgainstProfileNotExistingAsync(string username)
        {
            var exists = await DataContext.Profile.AnyAsync(x => x.Username == username);

            if (!exists)
                throw new NotFoundException("No profile matching that username");
        }

        public async Task<Domain.Models.Profile> GetProfileByUsernameAsync(string username)
        {
            var profile = await DataContext.Profile.FirstAsync(x => x.Username == username);

            return profile.ToModel();
        }
    }
}