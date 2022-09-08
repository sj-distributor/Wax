using Wax.Core.Entities.Customers;
using Wax.Core.Services.Customers.Exceptions;
using Wax.Messages.Commands.Customers;
using Wax.Messages.Events.Customers;

namespace Wax.Core.Services.Customers
{
    /// <summary>
    ///  service 之间的沟通
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerDataProvider _customerDataProvider;

        public CustomerService(ICustomerDataProvider customerDataProvider)
        {
            _customerDataProvider = customerDataProvider;
        }

        public async Task<CustomerCreatedEvent> CreateAsync(CreateCustomerCommand command)
        {
            var customer = await _customerDataProvider.GetByNameAsync(command.Name);

            if (customer != null)
            {
                throw new CustomerNameAlreadyExistsException();
            }

            customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = command.Name
            };

            await _customerDataProvider.AddAsync(customer);

            return new CustomerCreatedEvent(customer.Id, customer.Name);
        }
    }
}