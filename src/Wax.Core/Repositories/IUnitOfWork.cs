namespace Wax.Core.Repositories;

public interface IUnitOfWork : IDisposable
{
    public ICustomerRepository Customers { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}