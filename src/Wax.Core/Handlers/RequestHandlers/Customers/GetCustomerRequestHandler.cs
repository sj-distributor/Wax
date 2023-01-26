using AutoMapper;
using AutoMapper.QueryableExtensions;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Microsoft.EntityFrameworkCore;
using Wax.Core.Domain.Customers;
using Wax.Messages;
using Wax.Messages.Dtos.Customers;
using Wax.Messages.Requests.Customers;

namespace Wax.Core.Handlers.RequestHandlers.Customers;

public class GetCustomerRequestHandler : IRequestHandler<GetCustomerRequest, UniformResponse<CustomerShortInfo>>
{
    private readonly IMapper _mapper;
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerRequestHandler(IMapper mapper, ICustomerRepository customerRepository)
    {
        _mapper = mapper;
        _customerRepository = customerRepository;
    }

    public async Task<UniformResponse<CustomerShortInfo>> Handle(IReceiveContext<GetCustomerRequest> context,
        CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.Query.AsNoTracking()
            .Where(c => c.Id == context.Message.CustomerId)
            .ProjectTo<CustomerShortInfo>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        return UniformResponse<CustomerShortInfo>.Succeed(customer);
    }
}