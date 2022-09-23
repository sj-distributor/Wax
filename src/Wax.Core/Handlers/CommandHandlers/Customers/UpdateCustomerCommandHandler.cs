using AutoMapper;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Services.Customers;
using Wax.Core.Services.Customers.Exceptions;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers;

public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand>
{
    private readonly IMapper _mapper;
    private readonly ICustomerDataProvider _customerDataProvider;

    public UpdateCustomerCommandHandler(IMapper mapper, ICustomerDataProvider customerDataProvider)
    {
        _mapper = mapper;
        _customerDataProvider = customerDataProvider;
    }

    public async Task Handle(IReceiveContext<UpdateCustomerCommand> context, CancellationToken cancellationToken)
    {
        var customer = await _customerDataProvider.GetByIdAsync(context.Message.CustomerId);

        if (customer.Name != context.Message.Name)
        {
            if (!await _customerDataProvider.CheckIsUniqueNameAsync(context.Message.Name))
            {
                throw new CustomerNameAlreadyExistsException();
            }
        }

        _mapper.Map(context.Message, customer);

        await _customerDataProvider.UpdateAsync(customer);
    }
}