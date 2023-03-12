using Core.Models;

namespace Core.Interfaces
{
    public interface ICryptographicService
    {
        Password HashPassword(string password, string? salt = null);
    }
}