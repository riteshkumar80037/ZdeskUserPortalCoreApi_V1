using System;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZdeskUserPortal.Utility;
using ZdeskUserPortalApiCore.Common;
using ZdeskUserPortalApiCore.Constant;

namespace ZdeskUserPortalApiCore.Middleware
{
    /// <summary>
    /// Global middleware which will be invoked if any exception is triggered within the application
    /// </summary>
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(
            RequestDelegate next,
            ILogger<GlobalExceptionHandler> logger)
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
            catch (BusinessException ex)
            {
                ResponseMetaData<string> responseMetadata = new()
                {
                    Status = HttpStatusCode.InternalServerError,
                    IsError = true,
                    ErrorDetails = CommonExceptionConstants.SomeUnknownError
                };

                var serializedResponseMetadata = JsonConvert.SerializeObject(responseMetadata);
                _logger.LogError(ex, "Business error occurred: {Message}", JsonConvert.SerializeObject(serializedResponseMetadata));

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(serializedResponseMetadata);
            }
            catch (DataAccessException ex)
            {
                ResponseMetaData<string> responseMetadata = new()
                {
                    Status = HttpStatusCode.InternalServerError,
                    IsError = true,
                    ErrorDetails = CommonExceptionConstants.SomeUnknownError
                };

                var serializedResponseMetadata = JsonConvert.SerializeObject(responseMetadata);
                _logger.LogError(ex, "DataAccessException error occurred: {Message}", JsonConvert.SerializeObject(serializedResponseMetadata));

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(serializedResponseMetadata);
            }
            catch (Exception exception)
            {
                ResponseMetaData<string> responseMetadata = new()
                {
                    Status = HttpStatusCode.InternalServerError,
                    IsError = true,
                    ErrorDetails = CommonExceptionConstants.SomeUnknownError
                };

                var serializedResponseMetadata = JsonConvert.SerializeObject(responseMetadata);
                _logger.LogError(exception, "Exception occurred: {Message}", JsonConvert.SerializeObject(serializedResponseMetadata));

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(serializedResponseMetadata);
            }
        }

    }
}
