﻿using AutoMapper;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Domain.Customers;
using Wax.Core.Domain.Customers.Exceptions;
using Wax.Messages;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers
{
    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, UniformResponse<Guid>>
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _repository;

        public CreateCustomerCommandHandler(IMapper mapper, ICustomerRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<UniformResponse<Guid>> Handle(IReceiveContext<CreateCustomerCommand> context,
            CancellationToken cancellationToken)
        {
            if (!await _repository.CheckIsUniqueNameAsync(context.Message.Name, cancellationToken)
                    .ConfigureAwait(false))
            {
                throw new CustomerNameAlreadyExistsException();
            }

            var customer = _mapper.Map<Customer>(context.Message);

            await _repository.InsertAsync(customer, cancellationToken).ConfigureAwait(false);

            return UniformResponse<Guid>.Succeed(customer.Id);
        }
    }
}