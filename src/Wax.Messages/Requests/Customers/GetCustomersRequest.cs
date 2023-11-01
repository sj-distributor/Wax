using Mediator.Net.Contracts;

namespace Wax.Messages.Requests.Customers;

public class GetCustomersRequest : IRequest
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}