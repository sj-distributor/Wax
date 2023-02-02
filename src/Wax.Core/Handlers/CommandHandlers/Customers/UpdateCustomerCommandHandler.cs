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
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(IReceiveContext<UpdateCustomerCommand> context, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(context.Message.CustomerId, cancellationToken)
            .ConfigureAwait(false);

        if (customer.Name != context.Message.Name)
        {
            if (!await _unitOfWork.Customers.CheckIsUniqueNameAsync(context.Message.Name, cancellationToken)
                    .ConfigureAwait(false))
            {
                throw new CustomerNameAlreadyExistsException();
            }
        }

        _mapper.Map(context.Message, customer);

        await _unitOfWork.Customers.UpdateAsync(customer, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}