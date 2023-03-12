using Shared.Dtos.Requests;
using Shared.Dtos.Responses;

namespace Core.Interfaces
{
    public interface IProfileService
    {
        Task<AuthenticatedResponse> CreateAsync(RegisterProfileRequest request);
        Task<AuthenticatedResponse> LoginAsync(LoginProfileRequest request);
    }
}