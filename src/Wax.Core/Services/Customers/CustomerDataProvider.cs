using Microsoft.EntityFrameworkCore;
using Wax.Core.Data;
using Wax.Core.Domain.Customers;
using Wax.Core.Exceptions;

namespace Wax.Core.Services.Customers
{
    public class CustomerDataProvider : ICustomerDataProvider
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerDataProvider(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer> GetByIdAsync(Guid id)
        {
            var customer = await _dbContext.Set<Customer>().FindAsync(id);

            if (customer == null)
            {
                throw new EntityNotFoundException(typeof(Customer), id);
            }

            return customer;
        }

        public async Task<bool> CheckIsUniqueNameAsync(string name)
        {
            return !await _dbContext.Set<Customer>().AnyAsync(c => c.Name == name);
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            await _dbContext.Set<Customer>().AddAsync(customer).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return customer;
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return customer;
        }
    }
}