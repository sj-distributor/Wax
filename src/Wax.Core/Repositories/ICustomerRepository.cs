using Microsoft.EntityFrameworkCore;
using Wax.Core.Data;
using Wax.Core.DependencyInjection;
using Wax.Core.Domain.Customers;

namespace Wax.Core.Repositories;

public interface ICustomerRepository : IBasicRepository<Customer>
{
    Task<Customer> FindByNameAsync(string name);
}

public class CustomerRepository : BasicRepository<Customer>, ICustomerRepository, IScopedDependency
{
    public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public Task<Customer> FindByNameAsync(string name)
    {
        return Table.FirstOrDefaultAsync(c => c.Name == name);
    }
}