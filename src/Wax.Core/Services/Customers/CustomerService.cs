namespace Wax.Core.Services.Customers;

public class CustomerService : ICustomerService
{
    private readonly IApplicationDbContext _dbContext;

    public CustomerService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Customer> GetCustomerByIdAsync(Guid customerId)
    {
        var customer = await _dbContext.Customers.FindAsync(customerId);

        if (customer == null)
        {
            throw new WaxException($"Customer not found. Key: {customerId}.");
        }

        return customer;
    }

    public Task<IPaginatedList<Customer>> GetCustomersAsync(int pageIndex = 1, int pageSize = Int32.MaxValue)
    {
        return _dbContext.Customers.ToPaginatedListAsync(pageIndex, pageSize);
    }

    public Task<bool> IsUniqueCustomerNameAsync(string customerName)
    {
        return _dbContext.Customers.AllAsync(c => c.Name != customerName);
    }

    public async Task InsertCustomerAsync(Customer customer)
    {
        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.SaveChangesAsync();
    }

    public Task UpdateCustomerAsync(Customer customer)
    {
        return _dbContext.SaveChangesAsync();
    }

    public Task DeleteCustomerAsync(Customer customer)
    {
        _dbContext.Customers.Remove(customer);
        return _dbContext.SaveChangesAsync();
    }
}