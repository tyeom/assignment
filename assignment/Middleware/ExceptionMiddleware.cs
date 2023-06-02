using assignment.Middleware.Models;
using assignment.Services;
using System.Net;

namespace assignment.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerService _logger;
    public ExceptionMiddleware(RequestDelegate next, ILoggerService logger)
    {
        _logger = logger;
        _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex);
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync(new ExceptionDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = "Error from middleware.",
            DetailMessage = exception.Message,
        }.ToString());
    }
}
