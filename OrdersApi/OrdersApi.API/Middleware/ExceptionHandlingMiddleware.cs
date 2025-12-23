
using OrdersApi.Application.Common.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace OrdersApi.API.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
             
                await context.Response.WriteAsJsonAsync(new
                {
                    code = "validation_error",
                    message = "Validation failed.",
                    errors = new[] { ex.Message }
                });
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    code = "not_found",
                    message = ex.Message
                });
            }
            catch (BusinessRuleException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    code = "business_rule_violation",
                    message = ex.Message
                });
            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    code = "internal_error",
                    message = "An unexpected error occurred."
                });
            }
        }
    }

}
