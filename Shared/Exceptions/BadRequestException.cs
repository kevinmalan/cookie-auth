namespace Shared.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message, object? data = null)
            : base(message, data)
        {
        }
    }
}