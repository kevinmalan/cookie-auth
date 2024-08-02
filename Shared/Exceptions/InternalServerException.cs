namespace Shared.Exceptions
{
    public class InternalServerException(string message, object? data = null) : BaseException(message, data)
    {
    }
}
