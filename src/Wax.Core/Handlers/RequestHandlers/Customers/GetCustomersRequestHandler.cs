using FluentValidation;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Middlewares.FluentMessageValidator;
using Wax.Core.Repositories;
using Wax.Messages.Dtos.Customers;
using Wax.Messages.Requests;
using Wax.Messages.Requests.Customers;

namespace Wax.Core.Handlers.RequestHandlers.Customers;

public class GetCustomersRequestHandler : IRequestHandler<GetCustomersRequest, PaginatedResponse<CustomerShortInfo>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersRequestHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<PaginatedResponse<CustomerShortInfo>> Handle(IReceiveContext<GetCustomersRequest> context,
        CancellationToken cancellationToken)
    {
        var data = await _customerRepository.GetPaginatedListByProjectionAsync(
            c => new CustomerShortInfo
            {
                Id = c.Id,
                Address = c.Address,
                Name = c.Name
            },
            orderBy: o => o.Name,
            pageIndex: context.Message.PageIndex,
            pageSize: context.Message.PageSize,
            cancellationToken: cancellationToken);

        return new PaginatedResponse<CustomerShortInfo>(data);
    }
}

public class GetCustomersRequestValidator : FluentMessageValidator<GetCustomersRequest>
{
    public GetCustomersRequestValidator()
    {
        RuleFor(v => v.PageIndex).GreaterThan(0);
        RuleFor(v => v.PageSize).GreaterThan(0);
    }
}