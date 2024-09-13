namespace Shared.Exceptions
{
    public class ForbiddenException(string message, object? data = null) : BaseException(message, data)
    {
    }
}