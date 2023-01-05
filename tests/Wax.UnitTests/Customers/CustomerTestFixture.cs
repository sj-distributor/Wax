using AutoMapper;
using NSubstitute;
using Wax.Core.Domain.Customers;
using Wax.Core.Profiles;

namespace Wax.UnitTests.Customers;

public class CustomerTestFixture
{
    protected readonly IMapper Mapper;
    protected readonly ICustomerRepository Repository;

    protected CustomerTestFixture()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile(new CustomerProfile())).CreateMapper();
        Repository = Substitute.For<ICustomerRepository>();
    }
}