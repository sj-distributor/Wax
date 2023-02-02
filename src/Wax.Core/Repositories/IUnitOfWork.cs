namespace Wax.Core.Repositories;

public interface IUnitOfWork
{
    public ICustomerRepository Customers { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}