using DataLayer.EF.Entities;

namespace DataLayer.EF.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetLatestByProfileIdAsync(int profileId);
    }
}