using Wax.Core.Domain.Customers;

namespace Wax.Core.Repositories;

public interface ICustomerRepository : IBasicRepository<Customer>
{
    Task<Customer> FindByNameAsync(string name, CancellationToken cancellationToken = default);
}