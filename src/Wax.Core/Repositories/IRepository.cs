namespace Wax.Core.Repositories;

public interface IRepository
{
    public ICustomerRepository Customers { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}