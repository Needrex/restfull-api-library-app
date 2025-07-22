using System.Text.Json;
using RestApiApp.Exceptions;

namespace RestApiApp.Middlewares
{
    public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex switch
            {
                ArgumentException => 400,
                ConflictException => 409,
                NotFoundException => 400,
                KeyNotFoundException => 404,
                _ => 500
            };

                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    var result = new
                    {
                        type = ex.GetType().Name,
                        status = context.Response.StatusCode,
                        error = ex.Message,
                        detail = ex.InnerException?.Message,
                        treace = ex.StackTrace
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(result));
                }
                else
                {
                    var exceptionDetail = (context.Response.StatusCode == 500) ? "Internal Error"  : ex.InnerException?.Message;
                    var exceptionMessage = (context.Response.StatusCode == 500) ? "Internal Error"  : ex.Message;
                    var result = new
                    {
                        status = context.Response.StatusCode,
                        error = exceptionMessage,
                        detail = exceptionDetail
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(result));
                }
        }
    }
}
}
