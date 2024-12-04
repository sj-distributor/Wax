namespace Wax.Core.Services.Customers;

public interface ICustomerService: IScopedDependency
{
    Task<Customer> GetCustomerByIdAsync(Guid customerId);
    Task<IPaginatedList<Customer>> GetCustomersAsync(int pageIndex = 1, int pageSize = int.MaxValue);
    Task<bool> IsUniqueCustomerNameAsync(string customerName);
    Task InsertCustomerAsync(Customer customer);
    Task UpdateCustomerAsync(Customer customer);
    Task DeleteCustomerAsync(Customer customer);
}