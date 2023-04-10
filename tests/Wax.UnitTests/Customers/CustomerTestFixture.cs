using AutoMapper;
using NSubstitute;
using Wax.Core.Data;
using Wax.Core.Profiles;
using Wax.Core.Repositories;

namespace Wax.UnitTests.Customers;

public class CustomerTestFixture
{
    protected readonly IMapper Mapper;
    protected readonly ICustomerRepository Customers;

    protected CustomerTestFixture()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile(new CustomerProfile())).CreateMapper();
        Customers = Substitute.For<ICustomerRepository>();
    }
}