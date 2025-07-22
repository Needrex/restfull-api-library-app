using Microsoft.Extensions.Options;
using RestApiApp.Configurations;

namespace RestApiApp.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApiKeySettings _apiKey;
        public ApiKeyMiddleware(RequestDelegate next, IOptions<ApiKeySettings> options )
        {
            _next = next;
            _apiKey = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(_apiKey.Key, out var extractedKey) || extractedKey != _apiKey.Password)
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("API Key is invalid");
                return;
            }

            await _next(context);
        }
    }
}