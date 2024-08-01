namespace Shared.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message, object? data = null)
            : base(message, data)
        {
        }
    }
}