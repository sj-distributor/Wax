using Microsoft.EntityFrameworkCore;
using Wax.Core.Data;
using Wax.Core.DependencyInjection;
using Wax.Core.Domain.Customers;

namespace Wax.Core.Repositories;

public interface ICustomerRepository : IBasicRepository<Customer>, IScopedDependency
{
    Task<bool> IsUniqueAsync(string name);
}

public class CustomerRepository : BasicRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> IsUniqueAsync(string name)
    {
        return !await Table.AnyAsync(c => c.Name == name);
    }
}