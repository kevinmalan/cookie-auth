using Core.DomainModels;

namespace Core.Interfaces
{
    public interface ICryptographicService
    {
        Password HashPassword(string password);
    }
}