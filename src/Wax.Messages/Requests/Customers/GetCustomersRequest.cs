using Mediator.Net.Contracts;
using Wax.Messages.Dtos.Customers;

namespace Wax.Messages.Requests.Customers;

public class GetCustomerRequest : IRequest
{
    public Guid CustomerId { get; set; }
}

public class GetCustomerResponse : IResponse
{
    public CustomerShortInfo Customer { get; set; }
}