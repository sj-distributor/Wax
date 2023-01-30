using FluentValidation;
using Wax.Core.Processing.FluentMessageValidator;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Validators;


public class DeleteCustomerCommandValidator : FluentMessageValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator()
    {
        RuleFor(v => v.CustomerId).NotEmpty();
    }
}