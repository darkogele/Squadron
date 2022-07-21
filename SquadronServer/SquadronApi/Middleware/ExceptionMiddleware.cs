using System.Net;
using System.Text.Json;

namespace SquadronApi.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _environment;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(exception: ex, message: ex.Message);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = _environment.IsDevelopment()
            ? new ApiException(httpContext.Response.StatusCode, exception.Message, exception.StackTrace)
            : new ApiException(httpContext.Response.StatusCode, "Internal server Error");

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        var json = JsonSerializer.Serialize(response, jsonOptions);

        await httpContext.Response.WriteAsync(json);
    }

    public class ApiException
    {
        public ApiException(int statusCode, string? message = null, string? details = null)
        {
            StatusCode = statusCode;
            Message = message ?? string.Empty;
            Details = details ?? string.Empty;
        }

        public int StatusCode { get; init; }
        public string Message { get; init; }
        public string Details { get; init; }
    }
}