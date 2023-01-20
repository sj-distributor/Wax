using FluentValidation;
using Wax.Core.Middlewares.FluentMessageValidator;
using Wax.Messages.Requests.Customers;

namespace Wax.Core.Validators.Customers;

public class GetCustomerRequestValidator : FluentMessageValidator<GetCustomerRequest>
{
    public GetCustomerRequestValidator()
    {
        RuleFor(v => v.CustomerId).NotEmpty();
    }
}