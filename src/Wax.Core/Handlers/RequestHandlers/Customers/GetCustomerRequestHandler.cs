using AutoMapper;
using AutoMapper.QueryableExtensions;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Microsoft.EntityFrameworkCore;
using Wax.Core.Domain.Customers;
using Wax.Messages.Dtos.Customers;
using Wax.Messages.Requests.Customers;

namespace Wax.Core.Handlers.RequestHandlers.Customers;

public class GetCustomerRequestHandler : IRequestHandler<GetCustomerRequest, GetCustomerResponse>
{
    private readonly IMapper _mapper;
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerRequestHandler(IMapper mapper, ICustomerRepository customerRepository)
    {
        _mapper = mapper;
        _customerRepository = customerRepository;
    }

    public async Task<GetCustomerResponse> Handle(IReceiveContext<GetCustomerRequest> context,
        CancellationToken cancellationToken)
    {
        return new GetCustomerResponse
        {
            Customer = await _customerRepository.Query.AsNoTracking()
                .Where(c => c.Id == context.Message.CustomerId)
                .ProjectTo<CustomerShortInfo>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false)
        };
    }
}