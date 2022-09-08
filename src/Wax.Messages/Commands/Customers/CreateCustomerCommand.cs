using Mediator.Net.Contracts;

namespace Wax.Messages.Commands.Customers
{
    public class CreateCustomerCommand : ICommand
    {
        public string Name { get; set; }
    }
}
