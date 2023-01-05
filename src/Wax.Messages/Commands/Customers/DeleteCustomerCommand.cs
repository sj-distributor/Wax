using Mediator.Net.Contracts;

namespace Wax.Messages.Commands.Customers;

public class DeleteCustomerCommand : ICommand
{
    public Guid CustomerId { get; set; }
}