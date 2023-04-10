using AutoMapper;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Domain.Customers.Exceptions;
using Wax.Core.Repositories;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers;

public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand>
{
    private readonly IMapper _mapper;
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerCommandHandler(IMapper mapper, ICustomerRepository customerRepository)
    {
        _mapper = mapper;
        _customerRepository = customerRepository;
    }

    public async Task Handle(IReceiveContext<UpdateCustomerCommand> context, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(context.Message.CustomerId);

        if (customer.Name != context.Message.Name)
        {
            var existing = await _customerRepository.FindByNameAsync(context.Message.Name);

            if (existing != null)
            {
                throw new CustomerNameAlreadyExistsException();
            }
        }

        _mapper.Map(context.Message, customer);

        await _customerRepository.UpdateAsync(customer);
    }
}