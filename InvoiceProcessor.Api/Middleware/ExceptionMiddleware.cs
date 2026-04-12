using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceProcessor.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = exception switch
            {
                ArgumentException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var problem = new ProblemDetails
            {
                Title = "An error occurred while processing your request.",
                Status = statusCode,
                Detail = context.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment() ? exception.Message: "An unexpected error occurred",
                Instance = context.Request.Path,
                Type = $"https://httpstatuses.com/{statusCode}"
            };

            problem.Extensions["traceId"] = context.TraceIdentifier;

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsJsonAsync(problem);
        }
    }
}
