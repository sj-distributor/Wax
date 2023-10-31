using AutoMapper;
using FluentValidation;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Domain.Customers.Exceptions;
using Wax.Core.Middlewares.FluentMessageValidator;
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
        var customer = await _customerRepository.GetByIdAsync(context.Message.CustomerId, cancellationToken);

        if (customer.Name != context.Message.Name)
        {
            var isUnique = await _customerRepository.IsUniqueAsync(context.Message.Name);

            if (!isUnique)
            {
                throw new CustomerNameAlreadyExistsException();
            }
        }

        _mapper.Map(context.Message, customer);

        await _customerRepository.UpdateAsync(customer, cancellationToken);
    }
}

public class UpdateCustomerCommandValidator : FluentMessageValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(v => v.CustomerId).NotEmpty();
        RuleFor(v => v.Name).NotEmpty().MaximumLength(64);
        RuleFor(v => v.Address).MaximumLength(512);
        RuleFor(v => v.Contact).MaximumLength(128);
    }
}