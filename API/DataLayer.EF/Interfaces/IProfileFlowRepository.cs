using DataLayer.EF.Entities;

namespace DataLayer.EF.Interfaces
{
    public interface IProfileFlowRepository
    {
        Task CreateProfileAndRefreshTokenAsync(Profile profile, RefreshToken refreshToken);
        Task GuardAgainstExistingProfileAsync(string username);
        Task GuardAgainstProfileNotExistingAsync(string username);
        Task<Profile> GetProfileByUsernameAsync(string username);
    }
}