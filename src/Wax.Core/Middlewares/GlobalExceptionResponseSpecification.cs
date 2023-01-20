using FluentValidation;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Serilog;
using Wax.Core.Exceptions;
using Wax.Messages;
using Wax.Messages.Enums;

namespace Wax.Core.Middlewares;

public class GlobalExceptionResponseSpecification<TContext> : IPipeSpecification<TContext> where TContext : IContext<IMessage>
{
    private readonly ILogger _logger;

    public GlobalExceptionResponseSpecification(ILogger logger)
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
        switch (ex)
        {
            case BusinessException businessException:
                _logger.Warning(ex.Message);
                context.Result = UniformResponse.Failure(businessException.ErrorCode, businessException.Message);
                
                return Task.CompletedTask;
            
            case ValidationException validationException:
                _logger.Warning(string.Join(';', validationException.Errors.Select(e => e.ErrorMessage)));
                context.Result = UniformResponse.Failure(ErrorCode.NotFound, validationException.Message);
                
                return Task.CompletedTask;
        }

        _logger.Error(ex.Message);
        context.Result = UniformResponse.Failure(ErrorCode.Undefined, "An error occur.Try it again later.");
        
        return Task.CompletedTask;
    }
}