using Core.DomainModels;

namespace Core.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> CreateAsync(RefreshToken refreshToken);
    }
}