namespace Wax.Core.Mappings;

public class CustomerMapping : Profile
{
    public CustomerMapping()
    {
        CreateMap<Customer, CustomerShortInfo>();
        CreateMap<CreateCustomerCommand, Customer>();
    }
}