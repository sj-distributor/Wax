using System.Runtime.ExceptionServices;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Wax.Core.Middlewares.FluentMessageValidator;

public class MessageValidatorSpecification<TContext> : IPipeSpecification<TContext>
    where TContext : IContext<IMessage>
{
    private readonly IEnumerable<IFluentMessageValidator> _messageValidators;

    public MessageValidatorSpecification(IEnumerable<IFluentMessageValidator> messageValidators)
    {
        _messageValidators = messageValidators;
    }

    public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
    {
        return true;
    }

    public Task BeforeExecute(TContext context, CancellationToken cancellationToken)
    {
        return Task.WhenAll();
    }

    public Task Execute(TContext context, CancellationToken cancellationToken)
    {
        if (ShouldExecute(context, cancellationToken))
        {
            foreach (var validator in _messageValidators.Where(x => x.ForMessageType == context.Message.GetType()))
            {
                validator.ValidateMessage(context.Message);
            }
        }

        return Task.WhenAll();
    }

    public Task AfterExecute(TContext context, CancellationToken cancellationToken)
    {
        return Task.WhenAll();
    }

    public Task OnException(Exception ex, TContext context)
    {
        ExceptionDispatchInfo.Capture(ex).Throw();
        return Task.CompletedTask;
    }
}