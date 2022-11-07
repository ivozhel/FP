using System.Net;
using Newtonsoft.Json;
using PearlyWhites.Host.Middleware.CustomExceptions;

namespace PearlyWhites.Host.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> loger)
        {
            _next = next;
            _logger = loger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                switch (error)
                {
                    case CustomException e:
                        //custom app error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        //not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        //unhandled error
                        response.StatusCode = 500;
                        break;
                }
                var result = JsonConvert.SerializeObject(new { message = error.Message, stackTrace = error.StackTrace }, Formatting.Indented);
                _logger.LogError(result);
                await response.WriteAsync(result);
            }
        }
    }
}
