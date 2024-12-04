namespace Wax.Core.Commands.Customers;

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