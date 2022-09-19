using Wax.Core.Entities.Customers;

namespace Wax.Core.Services.Customers
{
    public interface ICustomerDataProvider
    {
        Task<Customer> GetByNameAsync(string name);
        Task<Customer> AddAsync(Customer customer);
    }
}