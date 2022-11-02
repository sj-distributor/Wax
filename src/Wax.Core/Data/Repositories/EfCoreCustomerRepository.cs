using Wax.Core.Domain.Customers;

namespace Wax.Core.Data.Repositories;

public class EfCoreCustomerRepository : EfCoreRepository<Customer, Guid>, ICustomerRepository
{
    public EfCoreCustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}