using Wax.Core.Services.Identity;

namespace Wax.IntegrationTests;

public class IntegrationTestUser : ICurrentUser
{
    public string Id => "_wax_test_user";
}