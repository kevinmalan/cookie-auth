using DataLayer.EF.Entities;

namespace DataLayer.EF.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<Domain.Models.RefreshToken> CreateIfNotExistsAsync(Domain.Models.RefreshToken refreshTokenModel, Guid profileLookupId);
    }
}