using System.Runtime.ExceptionServices;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Serilog;
using Wax.Core.Extensions;

namespace Wax.Core.Middlewares.Logging;

public class LoggerSpecification<TContext> : IPipeSpecification<TContext>
    where TContext : IContext<IMessage>
{
    private readonly ILogger _logger;

    public LoggerSpecification(ILogger logger)
    {
        _logger = logger;
    }

    public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
    {
        return true;
    }

    public Task BeforeExecute(TContext context, CancellationToken cancellationToken)
    {
        _logger.Information("----- Handling message {MessageName} ({@Message})", context.Message.GetGenericTypeName(),
            context.Message);
        return Task.CompletedTask;
    }

    public Task Execute(TContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task AfterExecute(TContext context, CancellationToken cancellationToken)
    {
        _logger.Information("----- Message {MessageName} handled - response: {@Response}",
            context.Message.GetGenericTypeName(), context.Result);
        
        return Task.CompletedTask;
    }

    public Task OnException(Exception ex, TContext context)
    {
        ExceptionDispatchInfo.Capture(ex).Throw();
        return Task.CompletedTask;
    }
}