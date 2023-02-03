using Autofac;
using Wax.Core.Data;

namespace Wax.Core.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IComponentContext _componentContext;

    public UnitOfWork(ApplicationDbContext context, IComponentContext componentContext)
    {
        _context = context;
        _componentContext = componentContext;
    }

    public ICustomerRepository Customers => _componentContext.Resolve<ICustomerRepository>();

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}