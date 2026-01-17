// DevQuestions.Web

using System.Text.Json;
using Shared;
using Shared.Exceptions;

namespace DevQuestions.Web.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(context: httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context: httpContext, exception: ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception: exception, message: exception.Message);

        var (code, errors) = exception switch
        {
            BadRequestException => (StatusCodes.Status500InternalServerError, JsonSerializer.Deserialize<Error[]>(json: exception.Message)),

            NotFoundException => (StatusCodes.Status404NotFound, JsonSerializer.Deserialize<Error[]>(json: exception.Message)),

            _ => (StatusCodes.Status500InternalServerError, [Error.Failure(null, "Something went wrong")]),
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = code;

        return context.Response.WriteAsJsonAsync(value: errors);
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this WebApplication app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}