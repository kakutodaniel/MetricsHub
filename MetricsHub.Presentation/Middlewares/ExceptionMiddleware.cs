using MetricsHub.Application.Exceptions;

namespace MetricsHub.Presentation.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApplicationValidationException ex)
            {
                _logger.LogWarning(ex,
                    "ExceptionMiddleware: Validation error on {Path} - Errors {Errors}",
                    context.Request.Path, string.Join(", ", ex.Errors.Values.SelectMany(x => x)));

                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = ex.Message,
                    errors = ex.Errors
                });
            }
            catch (DuplicateEventException ex)
            {
                _logger.LogWarning(ex,
                    "ExceptionMiddleware: Duplicate event on {Path}",
                    context.Request.Path);

                context.Response.StatusCode = 409;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = ex.Message
                });
            }
            catch (UnexpectedException ex)
            {
                _logger.LogWarning(ex,
                    "ExceptionMiddleware: Unexpected exception on {Path} - event: {event}",
                    context.Request.Path,
                    ex.Event);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "ExceptionMiddleware: Unhandled exception on {Path}",
                    context.Request.Path);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = "Unhandled error"
                });
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
