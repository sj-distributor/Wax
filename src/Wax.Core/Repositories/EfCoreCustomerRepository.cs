using Microsoft.EntityFrameworkCore;
using Wax.Core.Data;
using Wax.Core.DependencyInjection;
using Wax.Core.Domain.Customers;

namespace Wax.Core.Repositories;

public class EfCoreCustomerRepository : EfCoreBasicRepository<Customer>, ICustomerRepository, IScopedDependency
{
    private readonly DbSet<Customer> _customers;

    public EfCoreCustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _customers = dbContext.Set<Customer>();
    }

    public async Task<bool> CheckIsUniqueNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return !await _customers.AnyAsync(c => c.Name == name, cancellationToken).ConfigureAwait(false);
    }
}