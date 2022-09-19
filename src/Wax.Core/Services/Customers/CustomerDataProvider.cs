using Wax.Core.Entities.Customers;

namespace Wax.Core.Services.Customers
{
    public class CustomerDataProvider : ICustomerDataProvider
    {
        public Task<Customer> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> AddAsync(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}