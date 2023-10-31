using AutoMapper;
using FluentValidation;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Domain.Customers;
using Wax.Core.Domain.Customers.Exceptions;
using Wax.Core.Middlewares.FluentMessageValidator;
using Wax.Core.Repositories;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers;

public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, CreateCustomerResponse>
{
    private readonly IMapper _mapper;
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerCommandHandler(IMapper mapper, ICustomerRepository customerRepository)
    {
        _mapper = mapper;
        _customerRepository = customerRepository;
    }

    public async Task<CreateCustomerResponse> Handle(IReceiveContext<CreateCustomerCommand> context,
        CancellationToken cancellationToken)
    {
        var isUnique = await _customerRepository.IsUniqueAsync(context.Message.Name);

        if (!isUnique)
        {
            throw new CustomerNameAlreadyExistsException();
        }

        var customer = _mapper.Map<Customer>(context.Message);

        await _customerRepository.InsertAsync(customer, cancellationToken);

        return new CreateCustomerResponse { CustomerId = customer.Id };
    }
}

public class CreateCustomerCommandValidator : FluentMessageValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(v => v.Name).NotEmpty().MaximumLength(64);
        RuleFor(v => v.Address).MaximumLength(512);
        RuleFor(v => v.Contact).MaximumLength(128);
    }
}