using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Microsoft.EntityFrameworkCore;
using Wax.Core.Repositories;
using Wax.Messages.Dtos.Customers;
using Wax.Messages.Requests.Customers;

namespace Wax.Core.Handlers.RequestHandlers.Customers;

public class GetCustomerRequestHandler : IRequestHandler<GetCustomerRequest, GetCustomerResponse>
{
    private readonly IRepositoryProvider _provider;

    public GetCustomerRequestHandler(IRepositoryProvider provider)
    {
        _provider = provider;
    }

    public async Task<GetCustomerResponse> Handle(IReceiveContext<GetCustomerRequest> context,
        CancellationToken cancellationToken)
    {
        return new GetCustomerResponse
        {
            Data = await _provider.Customers.Table
                .Where(c => c.Id == context.Message.CustomerId)
                .Select(c => new CustomerShortInfo
                {
                    Id = c.Id,
                    Address = c.Address,
                    Name = c.Name
                })
                .FirstOrDefaultAsync(cancellationToken)
        };
    }
}