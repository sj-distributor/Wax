using Microsoft.EntityFrameworkCore;
using Wax.Core.Data;
using Wax.Core.Domain.Customers;

namespace Wax.Core.Services.Customers;

public class CustomerChecker : ICustomerChecker
{
    private readonly ApplicationDbContext _dbContext;

    public CustomerChecker(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<bool> CheckIsUniqueNameAsync(string name)
    {
        return !await _dbContext.Set<Customer>().AnyAsync(c => c.Name == name).ConfigureAwait(false);
    }
}