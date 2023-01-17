namespace Wax.Core.Domain.Customers;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<bool> CheckIsUniqueNameAsync(string name, CancellationToken cancellationToken = default);
}