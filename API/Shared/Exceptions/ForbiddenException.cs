namespace Shared.Exceptions
{
    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string message, object? data = null)
            : base(message, data)
        {
        }
    }
}