using AutoMapper;
using Wax.Core.Domain.Customers;
using Wax.Messages.Commands.Customers;
using Wax.Messages.Dtos.Customers;

namespace Wax.Core.Profiles;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerShortInfo>();
        CreateMap<CreateCustomerCommand, Customer>();
        CreateMap<UpdateCustomerCommand, Customer>();
    }
}