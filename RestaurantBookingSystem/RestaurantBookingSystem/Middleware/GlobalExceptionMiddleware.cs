using Microsoft.AspNetCore.Http;
using RestaurantBookingSystem.Utils;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestaurantBookingSystem.Middlewares
    {
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            ApiErrorResponse error;

            if (exception is AppException)
            {
                error = new ApiErrorResponse
                {
                    Success = false,
                    Message = exception.Message
                };
            }
            else
            {
                error = new ApiErrorResponse
                {
                    Success = false,
                    Message = exception.Message
                };

                Console.WriteLine("[SERVER ERROR] " + exception.ToString());
            }

            return context.Response.WriteAsync(
                JsonSerializer.Serialize(error, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                })
            );
        }
    }

}
