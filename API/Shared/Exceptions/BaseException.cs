namespace Shared.Exceptions
{
    public class BaseException : Exception
    {
        public object CustomData { get; set; }

        public BaseException(string message, object customData = null)
            : base(message)
        {
            CustomData = customData;
        }
    }
}