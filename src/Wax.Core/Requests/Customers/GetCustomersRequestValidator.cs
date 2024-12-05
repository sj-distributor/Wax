namespace Wax.Core.Requests.Customers;

public class GetCustomersRequestValidator : FluentMessageValidator<GetCustomersRequest>
{
    public GetCustomersRequestValidator()
    {
        RuleFor(v => v.PageIndex).GreaterThan(0);
        RuleFor(v => v.PageSize).GreaterThan(0);
    }
}