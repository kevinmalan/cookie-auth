using Domain.Models;

namespace Domain.Interfaces
{
    public interface ICryptographicService
    {
        Password HashPassword(string password, string? salt = null);
    }
}