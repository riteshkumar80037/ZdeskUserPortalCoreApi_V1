using System.Net;
using Newtonsoft.Json;
using ZdeskUserPortalApiCore.Common;
using ZdeskUserPortalApiCore.Constant;
using ZdeskUserPortalApiCore.Exceptions;

namespace ZdeskUserPortalApiCore.Middleware
{
    /// <summary>    
    /// Global middleware which will be invoked one any exception is triggered or custom exception is thrown within the application
    /// </summary>
    public class CustomExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandler> _logger;

        public CustomExceptionHandler(
            RequestDelegate next,
            ILogger<CustomExceptionHandler> logger)
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
            catch (Exception exception)
            {

                ResponseMetaData<string> responseMetadata = new()
                {
                    Status = HttpStatusCode.InternalServerError,
                    IsError = true,
                    ErrorDetails = CommonExceptionConstants.SomeUnknownError
                };

                if (exception is CustomApiException)
                {
                    responseMetadata.ErrorDetails = CommonExceptionConstants.CustomSomeUnknownError;
                }
                else
                {
                    responseMetadata.ErrorDetails = CommonExceptionConstants.SomeUnknownError;
                }

                var serializedResponseMetadata = JsonConvert.SerializeObject(responseMetadata);
                _logger.LogError(exception, "Exception occurred: {Message}", JsonConvert.SerializeObject(serializedResponseMetadata));

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(serializedResponseMetadata);
            }
        }
    }
}
