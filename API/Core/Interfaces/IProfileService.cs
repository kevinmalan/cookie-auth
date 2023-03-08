using Shared.Dtos.Requests;

namespace Core.Interfaces
{
    public interface IProfileService
    {
        Task CreateProfileAsync(RegisterProfileRequest request);
    }
}