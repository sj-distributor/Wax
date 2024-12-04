namespace Wax.Core.Requests.Customers;

public class GetCustomersRequest : IPaginatedRequest
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}