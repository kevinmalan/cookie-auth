using DataLayer.EF.Entities;

namespace DataLayer.EF.Interfaces
{
    public interface IProfileFlowRepository
    {
        Task CreateProfileAndRefreshTokenAsync(Domain.Models.Profile profileModel, Domain.Models.RefreshToken refreshTokenModel);
        Task GuardAgainstExistingProfileAsync(string username);
        Task GuardAgainstProfileNotExistingAsync(string username);
        Task<Domain.Models.Profile> GetProfileByUsernameAsync(string username);
    }
}