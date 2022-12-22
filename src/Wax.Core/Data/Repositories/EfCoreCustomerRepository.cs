using Microsoft.EntityFrameworkCore;
using Wax.Core.Domain.Customers;

namespace Wax.Core.Data.Repositories;

public class EfCoreCustomerRepository : EfCoreRepository<Customer>, ICustomerRepository
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