using Wax.Core.DependencyInjection;

namespace Wax.Core.Services.Customers;

public interface ICustomerChecker : IScopedDependency
{
    Task<bool> CheckIsUniqueNameAsync(string name);
}