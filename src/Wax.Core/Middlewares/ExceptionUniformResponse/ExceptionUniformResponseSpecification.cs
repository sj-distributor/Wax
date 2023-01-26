using FluentValidation;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Serilog;
using Wax.Core.Exceptions;
using Wax.Messages;
using Wax.Messages.Enums;

namespace Wax.Core.Middlewares.ExceptionUniformResponse;

public class ExceptionUniformResponseSpecification<TContext> : IPipeSpecification<TContext>
    where TContext : IContext<IMessage>
{
    private readonly ILogger _logger;

    public ExceptionUniformResponseSpecification(ILogger logger)
    {
        _logger = logger;
    }

    public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
    {
        return true;
    }

    public Task BeforeExecute(TContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Execute(TContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task AfterExecute(TContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task OnException(Exception ex, TContext context)
    {
        if (context.Result is null)
        {
            var resultDataType = Activator.CreateInstance(context.ResultDataType, true);
            context.Result = resultDataType;
        }

        if (context.Result is IUniformResponse response)
        {
            switch (ex)
            {
                case BusinessException businessException:
                    _logger.Warning(ex.Message);
                    response.Error = new ErrorReason(businessException.ErrorCode, businessException.Message);
                    break;
                case ValidationException validationException:
                    _logger.Warning(string.Join(';', validationException.Errors.Select(e => e.ErrorMessage)));
                    response.Error = new ErrorReason(ErrorCode.BadRequest, validationException.Message);
                    break;
                default:
                    _logger.Error(ex.Message);
                    response.Error = new ErrorReason(ErrorCode.Undefined, "An error occur. Try it again later.");
                    break;
            }

            return Task.CompletedTask;
        }
        else
        {
            throw new NotUniformResponseException(ex);
        }
    }
}