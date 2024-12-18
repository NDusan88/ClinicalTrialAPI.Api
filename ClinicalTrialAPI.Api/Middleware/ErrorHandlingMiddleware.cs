using Serilog;
using System.Net;

namespace ClinicalTrialAPI.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unhandled exception occurred while processing the request.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                InvalidOperationException => (int)HttpStatusCode.Conflict,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = statusCode;

            var response = new
            {
                statusCode = statusCode,
                message = exception switch
                {
                    ArgumentNullException => "Required data is missing in the request.",
                    KeyNotFoundException => "The requested resource was not found.",
                    InvalidOperationException => "The operation was invalid.",
                    UnauthorizedAccessException => "You are not authorized to access this resource.",
                    _ => "An internal server error occurred. Please try again later."
                },
                error = exception.Message,
                stackTrace = context.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() == true ? exception.StackTrace : null,
                path = context.Request.Path,
                requestId = context.TraceIdentifier,
                timestamp = DateTime.UtcNow
            };

            Log.Error("Response for error - StatusCode: {StatusCode}, Message: {Message}, Path: {Path}, RequestId: {RequestId}",
                response.statusCode, response.message, response.path, response.requestId);

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
