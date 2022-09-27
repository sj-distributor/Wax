using AutoMapper;
using AutoMapper.QueryableExtensions;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Microsoft.EntityFrameworkCore;
using Wax.Core.Data;
using Wax.Core.Domain.Customers;
using Wax.Messages.Dtos.Customers;
using Wax.Messages.Requests.Customers;

namespace Wax.Core.Handlers.RequestHandlers.Customers;

public class GetCustomerRequestHandler : IRequestHandler<GetCustomerRequest, GetCustomerResponse>
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _dbContext;

    public GetCustomerRequestHandler(IMapper mapper, ApplicationDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<GetCustomerResponse> Handle(IReceiveContext<GetCustomerRequest> context,
        CancellationToken cancellationToken)
    {
        return new GetCustomerResponse
        {
            Customer = await _dbContext.Set<Customer>()
                .Where(c => c.Id == context.Message.CustomerId)
                .ProjectTo<CustomerShortInfo>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false)
        };
    }
}