using Wax.Core.Data;

namespace Wax.Core.Repositories;

public interface IRepositoryProvider
{
    ICustomerRepository Customers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class RepositoryProvider : IRepositoryProvider
{
    private readonly ApplicationDbContext _context;

    public RepositoryProvider(ApplicationDbContext context)
    {
        _context = context;
    }

    private ICustomerRepository _customers;

    public ICustomerRepository Customers
    {
        get { return _customers ??= new CustomerRepository(_context); }
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}