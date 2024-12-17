namespace HealthCheck.Core.Exceptions
{
    public class AlreadyLastCategoryException : InvalidOperationException
    {
        public AlreadyLastCategoryException()
        {
        }

        public AlreadyLastCategoryException(string? message) : base(message)
        {
        }

        public AlreadyLastCategoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
