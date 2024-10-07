using Shared.Dtos.Responses;

namespace Domain.Interfaces
{
    public interface ICookieService
    {
        void IssueAuthCookies(AuthenticatedResponse tokens);

        Task RefreshAuthCookiesAsync();

        void DeleteAuthCookies();
    }
}