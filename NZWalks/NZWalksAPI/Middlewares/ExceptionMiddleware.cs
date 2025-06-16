using NZWalksAPI.Exceptions;

namespace NZWalksAPI.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            logger.LogError(ex, "Resource not found: {Message}", ex.Message);
            await HandleExceptionAsync(context, StatusCodes.Status404NotFound, ex.Message);
        }
        catch (ValidationException ex)
        {
            logger.LogError(ex, "Validation error: {Message}", ex.Message);
            await HandleExceptionAsync(context, StatusCodes.Status422UnprocessableEntity, ex.Message);
        }
        catch (BadRequestException ex)
        {
            logger.LogError(ex, "Bad request: {Message}", ex.Message);
            await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (UnauthorizedAccessAppException ex)
        {
            logger.LogError(ex, "Unauthorized access: {Message}", ex.Message);
            await HandleExceptionAsync(context, StatusCodes.Status401Unauthorized, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occurred.");
            await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError,
                "An unexpected error occurred.");
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var result = new { error = message, timestamp = DateTime.UtcNow };
        await context.Response.WriteAsJsonAsync(result);
    }
}