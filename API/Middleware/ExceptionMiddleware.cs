using System.Net;
using System.Text.Json;
using API.Common;
using Domain.Exceptions;

namespace API.Middleware;

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
        catch (ValidationException ex)
        {
            await WriteResponseAsync(context, HttpStatusCode.BadRequest,
                ApiResponse<object>.Fail("Validation failed", ex.Errors));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await WriteResponseAsync(context, HttpStatusCode.InternalServerError,
                ApiResponse<object>.Fail("An unexpected error occurred. Please try again later."));
        }
    }

    private static async Task WriteResponseAsync(HttpContext context, HttpStatusCode statusCode, object response)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }
}
