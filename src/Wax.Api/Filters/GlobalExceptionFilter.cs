using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Wax.Core.Exceptions;

namespace Wax.Api.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly Serilog.ILogger _logger;

    public GlobalExceptionFilter(Serilog.ILogger logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case BusinessException:
                HandleBusinessException(context);
                break;
            case EntityNotFoundException:
                HandleEntityNotFoundException(context);
                break;
            case FluentValidation.ValidationException:
                HandleValidationException(context);
                break;
            default:
                HandleInternalServerError(context);
                break;
        }

        context.ExceptionHandled = true;
    }

    private void HandleBusinessException(ExceptionContext context)
    {
        _logger.Warning(context.Exception.Message);

        var problemDetails = new ValidationProblemDetails
        {
            Instance = context.HttpContext.Request.Path,
            Status = StatusCodes.Status400BadRequest,
            Detail = "Please refer to the errors property for additional details."
        };

        problemDetails.Errors.Add("BusinessValidations", new string[] { context.Exception.Message });

        context.Result = new BadRequestObjectResult(problemDetails);
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }

    private void HandleInternalServerError(ExceptionContext context)
    {
        _logger.Error(context.Exception, context.Exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal error",
            Detail = "An error occur.Try it again."
        };

        context.Result = new ObjectResult(problemDetails);
    }

    private void HandleEntityNotFoundException(ExceptionContext context)
    {
        _logger.Warning(context.Exception.Message);

        var exception = (EntityNotFoundException)context.Exception;

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "The specified resource was not found.",
            Detail = exception.Message
        };

        context.Result = new NotFoundObjectResult(details);
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = context.Exception as FluentValidation.ValidationException;

        var details = new ValidationProblemDetails(exception.Errors.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray()));

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }
}