using AutoMapper;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Data;
using Wax.Core.Repositories;
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
        var customer = await _customerRepository.GetByIdAsync(context.Message.CustomerId);

        return new GetCustomerResponse
        {
            Customer = _mapper.Map<CustomerShortInfo>(customer)
        };
    }
}