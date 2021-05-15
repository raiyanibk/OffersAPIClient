using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OffersAPIClient.Utils.CustomException;
using OffersAPIClient.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OffersAPIClient.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var exceptionMessage = exception.Message != null ? exception.Message : exception.InnerException?.Message;
            var statusCode = (int)HttpStatusCode.InternalServerError;
            switch (exception.GetType().Name)
            {
                case nameof(UnauthorizedException):
                    statusCode = (int)HttpStatusCode.Unauthorized;

                    break;
                case nameof(BadRequestException):
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case nameof(NotFoundException):
                    statusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    exceptionMessage = "Something went wrong";
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            _logger.LogError($"{exception.Message}");

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = statusCode,
                Message = exceptionMessage
            }.ToString());
        }
    }
}
