using AutoMapper;
using NSubstitute;
using Wax.Core.Profiles;
using Wax.Core.Repositories;

namespace Wax.UnitTests.Customers;

public class CustomerTestFixture
{
    protected readonly IMapper Mapper;
    protected readonly ICustomerRepository Customers;
    protected readonly IRepository Repository;

    protected CustomerTestFixture()
    {
        Mapper = new MapperConfiguration(x => x.AddProfile(new CustomerProfile())).CreateMapper();
        Customers = Substitute.For<ICustomerRepository>();
        Repository = Substitute.For<IRepository>();
        Repository.Customers.Returns(Customers);
    }
}