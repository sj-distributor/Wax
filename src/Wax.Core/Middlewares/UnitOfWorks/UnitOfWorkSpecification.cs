using System.Runtime.ExceptionServices;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Wax.Core.Data;

namespace Wax.Core.Middlewares.UnitOfWorks;

public class UnitOfWorkSpecification<TContext> : IPipeSpecification<TContext>
    where TContext : IContext<IMessage>
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkSpecification(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
    {
        return context.Message is ICommand or IEvent;
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
        return ShouldExecute(context, cancellationToken)
            ? _unitOfWork.CommitAsync(cancellationToken)
            : Task.CompletedTask;
    }

    public Task OnException(Exception ex, TContext context)
    {
        ExceptionDispatchInfo.Capture(ex).Throw();
        return Task.CompletedTask;
    }
}