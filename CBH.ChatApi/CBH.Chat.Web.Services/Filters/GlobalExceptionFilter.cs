using System.Linq;
using System.Net;
using CBH.Chat.Infrastructure.Chat.Constants;
using CBH.Chat.Infrastructure.Chat.Exception;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CBH.Chat.Web.Services.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError($"{context.Exception} - {context.Exception?.Message} - {context.Exception?.InnerException} - {context.Exception?.StackTrace}");

            switch (context.Exception)
            {
                case InvalidRequestException invalidRequestException:
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Result = new BadRequestObjectResult(new
                    {
                        ErrorCode = ErrorConstants.ValidationErrorCode,
                        Message = $"{context.Exception?.Message} - {context.Exception?.InnerException}"
                    });
                    break;
                case ValidationException validationException:
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Result = new BadRequestObjectResult(new
                    {
                        ErrorCode = ErrorConstants.ValidationErrorCode,
                        Message = string.Join(",", validationException?.Errors?.Select(x => x.ErrorMessage))
                    });
                    break;
                default:
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Result = new BadRequestObjectResult(new
                    {
                        ErrorCode = ErrorConstants.GenericErrorCode,
                        Message = ErrorConstants.DefaultGenericErrorMessage
                    });
                    break;
            }

            context.ExceptionHandled = true;
        }
    }
}