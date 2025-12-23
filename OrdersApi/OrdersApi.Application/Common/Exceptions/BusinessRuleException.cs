
namespace OrdersApi.Application.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when a business rule is violated.
    /// This will be mapped to HTTP 409.
    /// </summary>
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message) { }
    }
}
