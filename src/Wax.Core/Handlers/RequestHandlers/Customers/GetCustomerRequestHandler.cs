using AutoMapper;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Repositories;
using Wax.Messages.Dtos.Customers;
using Wax.Messages.Requests.Customers;

namespace Wax.Core.Handlers.RequestHandlers.Customers;

public class GetCustomerRequestHandler : IRequestHandler<GetCustomerRequest, GetCustomerResponse>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomerRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetCustomerResponse> Handle(IReceiveContext<GetCustomerRequest> context,
        CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(context.Message.CustomerId, cancellationToken);

        return new GetCustomerResponse
        {
            Customer = _mapper.Map<CustomerShortInfo>(customer)
        };
    }
}