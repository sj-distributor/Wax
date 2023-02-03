using Microsoft.EntityFrameworkCore;
using Wax.Core.Data;
using Wax.Core.DependencyInjection;
using Wax.Core.Domain.Customers;

namespace Wax.Core.Repositories;

public class EfCoreCustomerRepository : EfCoreBasicRepository<Customer>, ICustomerRepository, IScopedDependency
{
    public EfCoreCustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public Task<Customer> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return Query(c => c.Name == name).FirstOrDefaultAsync(cancellationToken);
    }
}