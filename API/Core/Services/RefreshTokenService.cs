using Core.Contexts;
using Core.DomainModels;
using Core.Interfaces;

namespace Core.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly DataContext _dataContext;

        public RefreshTokenService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<RefreshToken> CreateAsync(RefreshToken refreshToken)
        {
            await _dataContext.AddAsync(refreshToken);
            await _dataContext.SaveChangesAsync();

            return refreshToken;
        }
    }
}