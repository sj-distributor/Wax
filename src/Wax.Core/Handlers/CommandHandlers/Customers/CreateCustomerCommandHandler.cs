﻿using AutoMapper;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Domain.Customers;
using Wax.Core.Services.Customers;
using Wax.Core.Services.Customers.Exceptions;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers
{
    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand>
    {
        private readonly IMapper _mapper;
        private readonly ICustomerDataProvider _customerDataProvider;

        public CreateCustomerCommandHandler(IMapper mapper, ICustomerDataProvider customerDataProvider)
        {
            _mapper = mapper;
            _customerDataProvider = customerDataProvider;
        }

        public async Task Handle(IReceiveContext<CreateCustomerCommand> context, CancellationToken cancellationToken)
        {
            if (!await _customerDataProvider.CheckIsUniqueNameAsync(context.Message.Name))
            {
                throw new CustomerNameAlreadyExistsException();
            }

            await _customerDataProvider.AddAsync(_mapper.Map<Customer>(context.Message));
        }
    }
}