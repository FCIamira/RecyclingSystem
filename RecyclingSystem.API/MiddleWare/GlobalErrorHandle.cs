using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace  RecyclingSystem.API.MiddleWare
{
    public class GlobalErrorHandle : IMiddleware
    {
        private readonly ILogger<GlobalErrorHandle> _logger;

        public GlobalErrorHandle(ILogger<GlobalErrorHandle> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred.");

            var errorResponse = new
            {
                IsSuccess = false,
                Data = (object?)null,
                ErrorCode = "ServerError",   
                Message = "Internal server error. Please try again later."
            };

            string json = JsonSerializer.Serialize(errorResponse);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(json);
        }
    }
}

