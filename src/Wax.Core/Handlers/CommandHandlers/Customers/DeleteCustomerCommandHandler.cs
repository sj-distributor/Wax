using FluentValidation;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Middlewares.FluentMessageValidator;
using Wax.Core.Repositories;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers;

public class DeleteCustomerCommandHandler: ICommandHandler<DeleteCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;

    public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    
    public async Task Handle(IReceiveContext<DeleteCustomerCommand> context, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(context.Message.CustomerId, cancellationToken);

        await _customerRepository.DeleteAsync(customer, cancellationToken);
    }
}

public class DeleteCustomerCommandValidator : FluentMessageValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator()
    {
        RuleFor(v => v.CustomerId).NotEmpty();
    }
}