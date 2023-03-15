using DataLayer.EF.Entities;

namespace DataLayer.EF.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<Domain.Models.RefreshToken> CreateAsync(Domain.Models.RefreshToken refreshTokenModel, Guid profileLookupId);

        Task<Domain.Models.RefreshToken> RenewAsync(string value, Guid profileLookupId, Guid accessTokenId);

        Task GuardAgainstExpiring(string value);
    }
}