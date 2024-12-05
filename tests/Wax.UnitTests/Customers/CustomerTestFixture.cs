namespace Wax.UnitTests.Customers;

public class CustomerTestFixture
{
    protected readonly Mock<ICustomerService> MockCustomerService;

    protected CustomerTestFixture()
    {
        AutoMapperConfiguration.Init(
            new MapperConfiguration(x => x.AddProfile(new CustomerMapping())).CreateMapper()
            .ConfigurationProvider);
        
        MockCustomerService = new Mock<ICustomerService>();
    }
}