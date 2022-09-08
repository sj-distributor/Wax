using Wax.Core.DependencyInjection;
using Wax.Messages.Commands.Customers;
using Wax.Messages.Events.Customers;

namespace Wax.Core.Services.Customers
{
    public interface ICustomerService : IScopedDependency
    {
        Task<CustomerCreatedEvent> CreateAsync(CreateCustomerCommand command);
    }
}
