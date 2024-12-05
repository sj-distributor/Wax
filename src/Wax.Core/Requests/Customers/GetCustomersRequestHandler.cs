namespace Wax.Core.Requests.Customers;

public class GetCustomersRequestHandler : IRequestHandler<GetCustomersRequest, PaginatedResponse<CustomerShortInfo>>
{
    private readonly ICustomerService _customerService;

    public GetCustomersRequestHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<PaginatedResponse<CustomerShortInfo>> Handle(IReceiveContext<GetCustomersRequest> context,
        CancellationToken cancellationToken)
    {
        var customers = await _customerService.GetCustomersAsync(context.Message.PageIndex, context.Message.PageSize);

        var data = customers.Select(c => c.ToModel<CustomerShortInfo>()).ToList();

        return data.ToPaginatedResponse(customers.TotalCount, context.Message);
    }
}