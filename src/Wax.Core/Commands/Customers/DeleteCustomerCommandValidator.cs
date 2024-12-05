namespace Wax.Core.Commands.Customers;

public class DeleteCustomerCommandValidator : FluentMessageValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator()
    {
        RuleFor(v => v.CustomerId).NotEmpty();
    }
}