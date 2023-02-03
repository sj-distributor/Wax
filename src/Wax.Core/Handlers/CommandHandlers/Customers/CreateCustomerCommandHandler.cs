using AutoMapper;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Domain.Customers;
using Wax.Core.Domain.Customers.Exceptions;
using Wax.Core.Repositories;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers
{
    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, CreateCustomerResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateCustomerResponse> Handle(IReceiveContext<CreateCustomerCommand> context,
            CancellationToken cancellationToken)
        {
            var existing = await _unitOfWork.Customers.FindByNameAsync(context.Message.Name, cancellationToken);

            if (existing != null)
            {
                throw new CustomerNameAlreadyExistsException();
            }

            var customer = _mapper.Map<Customer>(context.Message);

            await _unitOfWork.Customers.InsertAsync(customer, cancellationToken).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateCustomerResponse { CustomerId = customer.Id };
        }
    }
}