
namespace OrdersApi.Application.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested resource does not exist.
    /// This will be mapped to HTTP 404.
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
