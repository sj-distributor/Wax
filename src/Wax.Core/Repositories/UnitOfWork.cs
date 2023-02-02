using Autofac;
using Wax.Core.Data;

namespace Wax.Core.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly ILifetimeScope _lifetimeScope;

    public UnitOfWork(ApplicationDbContext context, ILifetimeScope lifetimeScope)
    {
        _context = context;
        _lifetimeScope = lifetimeScope;
    }

    public ICustomerRepository Customers => _lifetimeScope.Resolve<ICustomerRepository>();

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}