using Wax.Core.Domain.Customers;

namespace Wax.Core.Repositories;

public interface ICustomerRepository : IBasicRepository<Customer>
{
    Task<bool> CheckIsUniqueNameAsync(string name, CancellationToken cancellationToken = default);
}