using System.Net;
using System.Text.Json;
using Beneficiarios.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Beneficiarios.API.Middleware;

/// <summary>
/// Middleware that handles exceptions globally and returns standardized error responses.
/// </summary>
public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Intercepts requests and catches exceptions, returning appropriate HTTP responses.
    /// </summary>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            // Process the request through the pipeline
            await next(context);
        }
        catch (DomainValidationException ex)
        {
            // Handle domain validation exceptions with a 400 Bad Request response
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var payload = new { error = ex.Message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
        catch (Exception ex)
        {
            // Handle all other unhandled exceptions with a 500 Internal Server Error response
            _logger.LogError(ex, "An unhandled exception occurred");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var payload = new { error = "An unexpected error occurred." };
            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}
